namespace Botticelli.AI.Message;

public class AiMessage : Shared.ValueObjects.Message
{
    public AiMessage()
    {
    }

    public string Role { get; set; }

    public List<AiMessage> AdditionalMessages { get; set; }

    public AiMessage(string uid) : base(uid)
    {
    }
}