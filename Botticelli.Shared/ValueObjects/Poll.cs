namespace Botticelli.Shared.ValueObjects;

public class Poll
{
    public enum PollType
    {
        Quiz,
        Regular
    }

    public string? Question { get; set; }
    public IEnumerable<string>? Variants { get; set; }
    public int? CorrectAnswerId { get; set; }
    public bool IsAnonymous { get; set; }
    public PollType Type { get; set; }
}