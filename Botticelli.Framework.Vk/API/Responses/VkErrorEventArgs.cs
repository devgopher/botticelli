namespace Botticelli.Framework.Vk.API.Responses;

public class VkErrorEventArgs
{
    public VkErrorEventArgs(ErrorResponse response) => Response = response;

    public ErrorResponse Response { get; }
}