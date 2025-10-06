namespace Botticelli.Shared.API.Client.Requests;

public class DeleteMessageRequest : BaseRequest<DeleteMessageRequest>
{
    public DeleteMessageRequest(string? uid, string chatId) : base(uid)
    {
        ChatId = chatId;
    }

    public string? ChatId { get; set; }
}