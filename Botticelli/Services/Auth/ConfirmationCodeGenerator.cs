using System.Text;

namespace Botticelli.Server.Services.Auth;

public class ConfirmationCodeGenerator : IConfirmationCodeGenerator
{
    private readonly Random _rand = new(DateTime.Now.Millisecond);

    public string GenerateCode(int size = 4, int lifetimeSec = 600)
    {
        var code = new StringBuilder(size);

        for (var i = 0; i < size; ++i)
            code.Append(_rand.Next(0, 9));

        return code.ToString();
    }
}