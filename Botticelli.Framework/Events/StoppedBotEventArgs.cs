using MediatR;

namespace Botticelli.Framework.Events;

public class StoppedBotEventArgs : BotEventArgs, IRequest
{
}