using System;
using System.Threading;
using System.Threading.Tasks;
using Viber.Api.Requests;
using Viber.Api.Responses;

namespace Viber.Api
{
    public interface IViberService : IDisposable
    {
        delegate void GotMessageHandler(GetWebHookEvent @event);

        event GotMessageHandler GotMessage;

        void Start();

        void Stop();

        Task<SetWebHookResponse> SetWebHook(SetWebHookRequest request,
            CancellationToken cancellationToken = default);

        Task<ApiSendMessageResponse> SendMessage(ApiSendMessageRequest request,
            CancellationToken cancellationToken = default);
    }
}