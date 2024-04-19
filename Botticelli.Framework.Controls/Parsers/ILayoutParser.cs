using Botticelli.Framework.Controls.Layouts;

namespace Botticelli.Framework.Controls.Parsers;

public interface ILayoutParser
{
    public ILayout ParseJson(string jsonText);
}