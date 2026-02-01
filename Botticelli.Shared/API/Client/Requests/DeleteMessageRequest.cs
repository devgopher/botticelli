namespace Botticelli.Shared.API.Client.Requests;

public class DeleteMessageRequest(string? uid, string chatId) : BaseRequest<DeleteMessageRequest>(uid)
{
    public string? ChatId { get; set; } = chatId;
}