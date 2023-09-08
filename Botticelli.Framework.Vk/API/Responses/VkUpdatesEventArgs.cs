namespace Botticelli.Framework.Vk.API.Responses;

public class VkUpdatesEventArgs
{
    public VkUpdatesEventArgs(UpdatesResponse response) => Response = response;

    public UpdatesResponse Response { get; }
}