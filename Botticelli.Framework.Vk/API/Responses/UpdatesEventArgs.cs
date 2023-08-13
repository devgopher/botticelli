namespace Botticelli.Framework.Vk.API.Responses;

public class UpdatesEventArgs
{
    public UpdatesEventArgs(UpdatesResponse response) => Response = response;

    public UpdatesResponse Response { get; }
}