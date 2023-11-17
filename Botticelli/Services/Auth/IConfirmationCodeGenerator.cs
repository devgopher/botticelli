namespace Botticelli.Server.Services.Auth;

/// <summary>
///     Code generator
/// </summary>
public interface IConfirmationCodeGenerator
{
    /// <summary>
    ///     Generates a code
    /// </summary>
    /// <param name="size"></param>
    /// <param name="lifetimeSec"></param>
    /// <returns></returns>
    public string GenerateCode(int size = 4, int lifetimeSec = 600);
}