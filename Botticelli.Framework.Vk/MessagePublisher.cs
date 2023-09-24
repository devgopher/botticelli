using System.Net;
using Botticelli.Framework.Vk.Messages.API.Requests;
using Botticelli.Framework.Vk.Messages.API.Utils;
using Microsoft.Extensions.Logging;

namespace Botticelli.Framework.Vk.Messages;

public class MessagePublisher
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<MessagePublisher> _logger;
    private string _apiKey;

    public MessagePublisher(IHttpClientFactory httpClientFactory, ILogger<MessagePublisher> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    private string ApiVersion => "5.81";

    public void SetApiKey(string key)
        => _apiKey = key;


    public async Task SendAsync(VkSendMessageRequest vkMessageRequest, CancellationToken token)
    {
        try
        {
            vkMessageRequest.AccessToken = _apiKey;
            using var httpClient = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get,
                                                 ApiUtils.GetMethodUriWithJson("https://api.vk.com",
                                                                               "messages.send",
                                                                               vkMessageRequest));

            var response = await httpClient.SendAsync(request, token);
            if (response.StatusCode != HttpStatusCode.OK) _logger.LogError($"Error sending a message: {response.StatusCode} {response.ReasonPhrase}!");

            var responseContent = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(responseContent)) _logger.LogError($"Error sending a message {responseContent}");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error sending a message!");
        }
    }
}