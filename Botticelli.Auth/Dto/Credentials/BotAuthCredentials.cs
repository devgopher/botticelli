namespace Botticelli.Auth.Dto.Credentials;

public class BotAuthCredentials : IBotAuthCredentials
{
    public required string UserId { get; set; }
}