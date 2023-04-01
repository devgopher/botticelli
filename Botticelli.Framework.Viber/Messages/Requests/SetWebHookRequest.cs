namespace Botticelli.Framework.Viber.Messages.Requests;

public class SetWebHookRequest
{
    public string Url { get; set; }
    public List<string> EventTypes { get; set; }
    public bool SendName { get; set; }
    public bool SendPhoto { get; set; }
}