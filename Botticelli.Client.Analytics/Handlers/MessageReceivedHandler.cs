using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Botticelli.Framework.Events;
using MediatR;

namespace Botticelli.Client.Analytics.Handlers
{
    public class MessageReceivedHandler : IRequestHandler<MessageReceivedBotEventArgs>
    {
        public async Task Handle(MessageReceivedBotEventArgs request, CancellationToken cancellationToken) 
            => throw new NotImplementedException();
    }
}
