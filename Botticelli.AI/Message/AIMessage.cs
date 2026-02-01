namespace Botticelli.AI.Message;

public class AiMessage : Shared.ValueObjects.Message
{
    public AiMessage()
    {
    }

    public AiMessage(string uid) : base(uid)
    {
    }

    public string Instruction { get; set; } = string.Empty;

    public List<AiMessage> AdditionalMessages { get; set; } = new();
    
    public override Shared.ValueObjects.Message Copy()
    {
        var newMessage = (AiMessage)(base.Copy());
        newMessage.Instruction = Instruction;
        newMessage.AdditionalMessages = AdditionalMessages;
        
        return newMessage;
    }
}