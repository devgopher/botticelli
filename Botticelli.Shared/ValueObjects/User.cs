namespace Botticelli.Shared.ValueObjects;

public class User
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Info { get; set; }
    public string? NickName { get; set; }
    public bool? IsBot { get; set; }
}