﻿using Botticelli.Audio;
using Botticelli.BotBase.Exceptions;
using Botticelli.Framework.Vk.Messages.API.Requests;
using Botticelli.Framework.Vk.Messages.API.Responses;
using Botticelli.Framework.Vk.Messages.API.Utils;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using AudioMessage = Botticelli.Framework.Vk.Messages.API.Responses.AudioMessage;

namespace Botticelli.Framework.Vk.Messages;

/// <summary>
///     VK storage upload component
/// </summary>
public class VkStorageUploader
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConvertor _audioConvertor;
    private readonly ILogger<MessagePublisher> _logger;
    private string _apiKey;

    public VkStorageUploader(IHttpClientFactory httpClientFactory, 
        IConvertor audioConvertor,
        ILogger<MessagePublisher> logger)
    {
        _httpClientFactory = httpClientFactory;
        _audioConvertor = audioConvertor;
        _logger = logger;
    }

    private string ApiVersion => "5.131";

    public void SetApiKey(string key) => _apiKey = key;

    /// <summary>
    ///     Get an upload address for a photo
    /// </summary>
    /// <param name="vkMessageRequest"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    private async Task<GetUploadAddress?> GetPhotoUploadAddress(VkSendMessageRequest vkMessageRequest, CancellationToken token)
    {
        try
        {
            using var httpClient = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get,
                                                 ApiUtils.GetMethodUri("https://api.vk.com",
                                                                       "photos.getMessagesUploadServer",
                                                                       new
                                                                       {
                                                                           access_token = _apiKey,
                                                                           v = ApiVersion,
                                                                           peer_id = vkMessageRequest.PeerId
                                                                       }));

            var response = await httpClient.SendAsync(request, token);
            var resultString = await response.Content.ReadAsStringAsync(token);

            return JsonSerializer.Deserialize<GetUploadAddress>(resultString);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting an upload address!");
        }

        return default;
    }



    /// <summary>
    ///     Get an upload address for an audio
    /// </summary>
    /// <param name="vkMessageRequest"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    private async Task<GetUploadAddress?> GetAudioUploadAddress(VkSendMessageRequest vkMessageRequest, CancellationToken token)
        => await GetDocsUploadAddress(vkMessageRequest, "audio_message", token);

    private async Task<GetUploadAddress?> GetDocsUploadAddress(VkSendMessageRequest vkMessageRequest, string type, CancellationToken token)
    {
        try
        {
            using var httpClient = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get,
                                                 ApiUtils.GetMethodUri("https://api.vk.com",
                                                                       "docs.getMessagesUploadServer",
                                                                       new
                                                                       {
                                                                           access_token = _apiKey,
                                                                           v = ApiVersion,
                                                                           peer_id = vkMessageRequest.PeerId,
                                                                           type = type
                                                                       }));

            var response = await httpClient.SendAsync(request, token);
            var resultString = await response.Content.ReadAsStringAsync(token);

            return JsonSerializer.Deserialize<GetUploadAddress>(resultString);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting an upload address!");
        }

        return default;
    }

    /// <summary>
    ///     Get an upload address for a video
    ///     seems to be prohibited for bots in communities
    /// </summary>
    /// <param name="vkMessageRequest"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    //private async Task<VkSendVideoResponse?> GetVideoUploadData(VkSendMessageRequest vkMessageRequest, CancellationToken token)
    //{
    //    try
    //    {
    //        using var httpClient = _httpClientFactory.CreateClient();
    //        var request = new HttpRequestMessage(HttpMethod.Get,
    //                                             ApiUtils.GetMethodUri("https://api.vk.com",
    //                                                                   "video.save",
    //                                                                   new
    //                                                                   {
    //                                                                       access_token = _apiKey,
    //                                                                       v = ApiVersion
    //                                                                   }));

    //        var response = await httpClient.SendAsync(request, token);
    //        var resultString = await response.Content.ReadAsStringAsync(token);

    //        return JsonSerializer.Deserialize<VkSendVideoResponse>(resultString);
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex, $"Error getting an upload address!");
    //    }

    //    return default;
    //}

    /// <summary>
    ///     Uploads a photo (binaries)
    /// </summary>
    /// <param name="uploadUrl"></param>
    /// <param name="binaryContent"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    private async Task<UploadPhotoResult> UploadPhoto(string uploadUrl, string name, byte[] binaryContent, CancellationToken token)
    {
        using var httpClient = _httpClientFactory.CreateClient();
        using var memoryContentStream = new MemoryStream(binaryContent);
        memoryContentStream.Seek(0, SeekOrigin.Begin);

        var content = new MultipartFormDataContent {{new StreamContent(memoryContentStream), "photo", name}};

        var response = await httpClient.PostAsync(uploadUrl, content, token);
        var resultString = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<UploadPhotoResult>(resultString);
    }


    /// <summary>
    ///     Uploads an audio message (binaries)
    /// </summary>
    /// <param name="uploadUrl"></param>
    /// <param name="binaryContent"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    private async Task<UploadDocResult> UploadAudioMessage(string uploadUrl, string name, byte[] binaryContent, CancellationToken token)
    {
        // convert to ogg in order to meet VK requirements
        var oggContent = _audioConvertor.Convert(binaryContent, new AudioInfo()
        {
            AudioFormat = AudioFormat.Ogg,
            Bitrate = 16000
        });

        return await PushDocument<UploadDocResult>(uploadUrl, name, oggContent, token);
    }

    private async Task<TResult> PushDocument<TResult>(string uploadUrl, string name, byte[] binContent,
        CancellationToken token)
    {
        using var httpClient = _httpClientFactory.CreateClient();
        var content = new MultipartFormDataContent
        {
            {
                new ByteArrayContent(binContent, 0, binContent.Length),
                "file",
                $"{Path.GetFileNameWithoutExtension(name)}{Guid.NewGuid()}.{Path.GetExtension(name)}"
            }
        };

        var response = await httpClient.PostAsync(uploadUrl, content, token);
        var resultString = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<TResult>(resultString);
    }

    /// <summary>
    ///     Uploads a document message (binaries)
    /// </summary>
    /// <param name="uploadUrl"></param>
    /// <param name="binaryContent"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    private async Task<UploadDocResult> UploadDocMessage(string uploadUrl, string name, byte[] binaryContent, CancellationToken token) 
        => await PushDocument<UploadDocResult>(uploadUrl, name, binaryContent, token);

    /// <summary>
    ///     Uploads a video (binaries)
    /// </summary>
    /// <param name="uploadUrl"></param>
    /// <param name="binaryContent"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    private async Task<UploadVideoResult> UploadVideo(string uploadUrl, string name, byte[] binaryContent, CancellationToken token)
    {
        using var httpClient = _httpClientFactory.CreateClient();
        using var memoryContentStream = new MemoryStream(binaryContent);
        memoryContentStream.Seek(0, SeekOrigin.Begin);

        var content = new MultipartFormDataContent { { new StreamContent(memoryContentStream), "video", name } };

        var response = await httpClient.PostAsync(uploadUrl, content, token);
        var resultString = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<UploadVideoResult>(resultString);
    }

    /// <summary>
    ///     The main public method for sending a photo
    /// </summary>
    /// <param name="vkMessageRequest"></param>
    /// <param name="binaryContent"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    /// <exception cref="BotException"></exception>
    public async Task<VkSendPhotoResponse> SendPhotoAsync(VkSendMessageRequest vkMessageRequest, string name, byte[] binaryContent, CancellationToken token)
    {
        try
        {
            var address = await GetPhotoUploadAddress(vkMessageRequest, token);

            if (address?.Response == default) throw new BotException("Sending photo error: no upload server address!");

            var uploadedPhoto = await UploadPhoto(address.Response.UploadUrl, name, binaryContent, token);

            if (uploadedPhoto?.Photo == default) throw new BotException("Sending photo error: no media uploaded!");

            using var httpClient = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Post,
                                                 ApiUtils.GetMethodUri("https://api.vk.com",
                                                                       "photos.saveMessagesPhoto",
                                                                       new
                                                                       {
                                                                           server = uploadedPhoto.Server,
                                                                           hash = uploadedPhoto.Hash,
                                                                           access_token = _apiKey,
                                                                           v = ApiVersion
                                                                       }));
            request.Content = ApiUtils.GetMethodMultipartFormContent(new
            {
                photo = uploadedPhoto.Photo
            }, 
            snakeCase:true);

            var response = await httpClient.SendAsync(request, token);
            var resultString = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<VkSendPhotoResponse>(resultString);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading media");
        }

        return default;
    }


    /// <summary>
    ///     The main public method for sending an audio message
    /// </summary>
    /// <param name="vkMessageRequest"></param>
    /// <param name="binaryContent"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    /// <exception cref="BotException"></exception>
    public async Task<VkSendAudioResponse> SendAudioMessageAsync(VkSendMessageRequest vkMessageRequest, string name, byte[] binaryContent, CancellationToken token)
    {
        try
        {
            var address = await GetAudioUploadAddress(vkMessageRequest, token);

            if (address?.Response == default) throw new BotException("Sending audio error: no upload server address!");

            var uploadedAudio = await UploadAudioMessage(address.Response.UploadUrl, name, binaryContent, token);

            if (uploadedAudio?.File == default) throw new BotException("Sending audio error: no media uploaded!");

            using var httpClient = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Post,
                                                 ApiUtils.GetMethodUri("https://api.vk.com",
                                                                       "docs.save",
                                                                       new
                                                                       {
                                                                           file = uploadedAudio.File,
                                                                           access_token = _apiKey,
                                                                           v = ApiVersion
                                                                       }));
            request.Content = ApiUtils.GetMethodMultipartFormContent(new
            {
                audio = uploadedAudio.File
            },
            snakeCase: true);

            var response = await httpClient.SendAsync(request, token);
            var resultString = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<VkSendAudioResponse>(resultString);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading media");
        }

        return default;
    }


    public async Task<VkSendDocumentResponse> SendDocsMessageAsync(VkSendMessageRequest vkMessageRequest, string name, byte[] binaryContent, CancellationToken token)
    {
        try
        {
            var address = await GetDocsUploadAddress(vkMessageRequest, "doc", token);

            if (address?.Response == default) throw new BotException("Sending doc error: no upload server address!");

            var uploadedDoc = await UploadDocMessage(address.Response.UploadUrl, name, binaryContent, token);

            if (uploadedDoc?.File == default) throw new BotException("Sending doc error: no file uploaded!");

            using var httpClient = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Post,
                                                 ApiUtils.GetMethodUri("https://api.vk.com",
                                                                       "docs.save",
                                                                       new
                                                                       {
                                                                           file = uploadedDoc.File,
                                                                           access_token = _apiKey,
                                                                           v = ApiVersion
                                                                       }));
            request.Content = ApiUtils.GetMethodMultipartFormContent(new
            {
                doc = uploadedDoc.File
            },
            snakeCase: true);

            var response = await httpClient.SendAsync(request, token);
            var resultString = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<VkSendDocumentResponse>(resultString);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading media");
        }

        return default;
    }

    /// <summary>
    ///     The main public method for sending a video
    ///     seems to be prohibited for bots in communities
    /// </summary>
    /// <param name="vkMessageRequest"></param>
    /// <param name="binaryContent"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    /// <exception cref="BotException"></exception>
    //public async Task<VkSendVideoResponse> SendVideoAsync(VkSendMessageRequest vkMessageRequest, string name, byte[] binaryContent, CancellationToken token)
    //{
    //    try
    //    {
    //        var sendVideoData = await GetVideoUploadData(vkMessageRequest, token);

    //        if (sendVideoData?.AudioResponseData == default) throw new BotException("Sending video error: no upload server address!");

    //        var uploadedVideo = await UploadVideo(sendVideoData.AudioResponseData.UploadUrl, name, binaryContent, token);

    //        if (uploadedVideo?.Video == default) throw new BotException("Sending video error: no media uploaded!");

    //        return sendVideoData;
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex, "Error uploading media");
    //    }

    //    return default;
    //}
}