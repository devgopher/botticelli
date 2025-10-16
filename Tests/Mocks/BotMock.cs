using Botticelli.BotData.Entities.Bot;
using Botticelli.Interfaces;
using Botticelli.Shared.API;
using Botticelli.Shared.API.Admin.Requests;
using Botticelli.Shared.API.Admin.Responses;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;
using Botticelli.Shared.Constants;

namespace Mocks;

public class BotMock : IBot<BotMock>
{
    public Task<StartBotResponse> StartBotAsync(StartBotRequest request, CancellationToken token) =>
        Task.FromResult(StartBotResponse.GetInstance(AdminCommandStatus.Ok, "Started"));

    public Task<StopBotResponse> StopBotAsync(StopBotRequest request, CancellationToken token) =>
        Task.FromResult(StopBotResponse.GetInstance(AdminCommandStatus.Ok, "Stopped"));

    public Task SetBotContext(BotData? context, CancellationToken token) => Task.CompletedTask;

    public Task<SendMessageResponse> SendMessageAsync(SendMessageRequest request, CancellationToken token) =>
        Task.FromResult(SendMessageResponse.GetInstance(Guid.NewGuid().ToString(), "OK"));

    public Task<SendMessageResponse> SendMessageAsync<TSendOptions>(SendMessageRequest request,
        ISendOptionsBuilder<TSendOptions>? optionsBuilder,
        CancellationToken token) where TSendOptions : class =>
        Task.FromResult(SendMessageResponse.GetInstance(Guid.NewGuid().ToString(), "OK"));

    public Task<SendMessageResponse> UpdateMessageAsync(SendMessageRequest request, CancellationToken token) =>
        Task.FromResult(SendMessageResponse.GetInstance(Guid.NewGuid().ToString(), "OK"));

    public Task<SendMessageResponse> UpdateMessageAsync<TSendOptions>(SendMessageRequest request,
        ISendOptionsBuilder<TSendOptions>? optionsBuilder,
        CancellationToken token) where TSendOptions : class =>
        Task.FromResult(SendMessageResponse.GetInstance(Guid.NewGuid().ToString(), "OK"));

    public Task<RemoveMessageResponse> DeleteMessageAsync(DeleteMessageRequest request, CancellationToken token) =>
        Task.FromResult(RemoveMessageResponse.GetInstance(Guid.NewGuid().ToString(), "OK"));

    public BotType Type { get; }
    public string? BotUserId { get; set; }
}