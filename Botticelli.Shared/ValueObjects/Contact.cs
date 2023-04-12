namespace Botticelli.Shared.ValueObjects;

public class Contact
{
    public string? Phone { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
}

public class Poll
{
    public enum PollType
    {
        Quiz,
        Regular
    }

    public string? Question { get; set; }
    public IEnumerable<string>? Variants { get; set; }
    public bool IsAnonymous { get; set; }
    public PollType Type { get; set; }
}