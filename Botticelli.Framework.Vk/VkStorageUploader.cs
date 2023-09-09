using System.Text.Json;
using Botticelli.BotBase.Exceptions;
using Botticelli.Framework.Vk.API.Requests;
using Botticelli.Framework.Vk.API.Responses;
using Botticelli.Framework.Vk.API.Utils;
using Microsoft.Extensions.Logging;

namespace Botticelli.Framework.Vk.Messages;

/// <summary>
///     VK storage upload component
/// </summary>
public class VkStorageUploader
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<MessagePublisher> _logger;
    private string _apiKey;
    private string ApiVersion => "5.81";

    public void SetApiKey(string key) => _apiKey = key;

    /// <summary>
    ///     Get an upload address for a photo
    /// </summary>
    /// <param name="vkMessageRequest"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    private async Task<GetUploadAddress?> GetPhotoUploadAddress(VkSendMessageRequest vkMessageRequest, CancellationToken token)
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
        var resultString = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<GetUploadAddress>(resultString);
    }

    /// <summary>
    ///     Uploads a photo (binaries)
    /// </summary>
    /// <param name="uploadUrl"></param>
    /// <param name="binaryContent"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    private async Task<UploadPhotoResult> UploadPhoto(string uploadUrl, byte[] binaryContent, CancellationToken token)
    {
        using var httpClient = _httpClientFactory.CreateClient();
        using var memoryContentStream = new MemoryStream(binaryContent);
        memoryContentStream.Seek(0, SeekOrigin.Begin);

        var content = new MultipartFormDataContent
        {
            new StreamContent(memoryContentStream)
        };

        var response = await httpClient.PostAsync(uploadUrl, content, token);
        var resultString = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<UploadPhotoResult>(resultString);
    }

    /// <summary>
    ///     The main public method for sending a photo
    /// </summary>
    /// <param name="vkMessageRequest"></param>
    /// <param name="binaryContent"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    /// <exception cref="BotException"></exception>
    public async Task<VkSendPhotoResponse> SendPhoto(VkSendMessageRequest vkMessageRequest, byte[] binaryContent, CancellationToken token)
    {
        var address = await GetPhotoUploadAddress(vkMessageRequest, token);

        if (address?.Response == default) throw new BotException("Sending photo error: no uploadserver address!");

        var uploadedPhoto = await UploadPhoto(address.Response.UploadUrl, binaryContent, token);

        if (uploadedPhoto?.Response == default) throw new BotException("Sending photo error: no upload error!");

        using var httpClient = _httpClientFactory.CreateClient();
        var request = new HttpRequestMessage(HttpMethod.Get,
                                             ApiUtils.GetMethodUri("https://api.vk.com",
                                                                   "photos.saveMessagesPhoto",
                                                                   new
                                                                   {
                                                                       access_token = _apiKey,
                                                                       v = ApiVersion,
                                                                       photo = uploadedPhoto.Response
                                                                   }));

        var response = await httpClient.SendAsync(request, token);
        var resultString = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<VkSendPhotoResponse>(resultString);
    }
}