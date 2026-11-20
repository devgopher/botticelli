using Botticelli.BotData.Entities.Bot;
using Botticelli.Framework;
using Botticelli.Interfaces;
using Botticelli.Shared.API;
using Botticelli.Shared.API.Admin.Requests;
using Botticelli.Shared.API.Admin.Responses;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;
using Botticelli.Shared.Constants;
using Microsoft.Extensions.Logging;

namespace Botticelli.Broadcasting.Tests.Helpers;

public sealed class TestBroadcastBot : BaseBot<TestBroadcastBot>, IBot<TestBroadcastBot>
{
    public TestBroadcastBot(ILogger logger) : base(logger, null)
    {
    }

    public override BotType Type => BotType.Telegram;

    public override Task SetBotContext(Botticelli.BotData.Entities.Bot.BotData? context, CancellationToken token)
        => Task.CompletedTask;

    protected override Task<StartBotResponse> InnerStartBotAsync(StartBotRequest request, CancellationToken token)
        => Task.FromResult(StartBotResponse.GetInstance(request.Uid, "ok", AdminCommandStatus.Ok));

    protected override Task<StopBotResponse> InnerStopBotAsync(StopBotRequest request, CancellationToken token)
        => Task.FromResult(StopBotResponse.GetInstance(request.Uid, "ok", AdminCommandStatus.Ok));

    protected override Task<SendMessageResponse> InnerSendMessageAsync<TSendOptions>(
        SendMessageRequest request,
        ISendOptionsBuilder<TSendOptions>? optionsBuilder,
        bool isUpdate,
        CancellationToken token)
        => Task.FromResult(new SendMessageResponse(request.Uid, "ok")
        {
            MessageSentStatus = MessageSentStatus.Ok
        });

    protected override Task<RemoveMessageResponse> InnerDeleteMessageAsync(
        DeleteMessageRequest request,
        CancellationToken token)
        => Task.FromResult(RemoveMessageResponse.GetInstance(request.Uid, "ok"));
}
