namespace Botticelli.AI.Message
{
    public class AIMessage : Shared.ValueObjects.Message
    {
        public AIMessage(string uid) : base(uid)
        {
        }

        public string? AiName { get; set; }
    }
}
