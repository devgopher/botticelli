using Botticelli.Framework.Vk;
using Botticelli.Interfaces;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.ValueObjects;
using Microsoft.Extensions.Options;
using VkMessagingSample.Settings;

namespace VkMessagingSample;

/// <summary>
///     This hosted service intended for sending messages according to a schedule
/// </summary>
public class TestBotHostedService : IHostedService
{
    private readonly IOptionsMonitor<SampleSettings> _settings;
    private readonly IBot<VkBot> _vkBot;

    public TestBotHostedService(IBot<VkBot> vkBot, IOptionsMonitor<SampleSettings> settings)
    {
        _vkBot = vkBot;
        _settings = settings;
    }

    public Task StartAsync(CancellationToken token)
    {
        Console.WriteLine("Start sending messages...");

        _vkBot.SendMessageAsync(new SendMessageRequest(Guid.NewGuid().ToString())
                                {
                                    Message = new Message(Guid.NewGuid().ToString())
                                    {
                                        Body = "dewwedewde",
                                        ChatIds = new List<string>()
                                        {
                                            "221973506"
                                        }
                                    }
                                },
                                CancellationToken.None);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Stop sending messages...");

        return Task.CompletedTask;
    }
}