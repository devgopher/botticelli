using System.Net;
using Botticelli.Framework.Vk.API.Requests;
using Botticelli.Framework.Vk.API.Utils;
using Microsoft.Extensions.Logging;

namespace Botticelli.Framework.Vk;

public class MessagePublisher
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<MessagePublisher> _logger;

    public MessagePublisher(IHttpClientFactory httpClientFactory, ILogger<MessagePublisher> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task SendAsync(SendMessageRequest vkMessageRequest, CancellationToken token)
    {
        try
        {
            using var httpClient = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get,
                                                 ApiUtils.GetMethodUri("https://api.vk.com",
                                                                       "messages.getLongPollServer",
                                                                       vkMessageRequest));

            var response = await httpClient.SendAsync(request, token);
            if (response.StatusCode != HttpStatusCode.OK) _logger.LogError($"Error sending a message: {response.StatusCode} {response.ReasonPhrase}!");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error sending a message!");
        }
    }
}