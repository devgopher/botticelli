namespace Botticelli.Framework.Vk.Messages.API.Responses;

public class VkUpdatesEventArgs
{
    public VkUpdatesEventArgs(UpdatesResponse response) => Response = response;

    public UpdatesResponse Response { get; }
}