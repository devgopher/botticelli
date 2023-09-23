using MediatR;

namespace Botticelli.Framework.Events;

public class StartedBotEventArgs : BotEventArgs, IRequest
{
}