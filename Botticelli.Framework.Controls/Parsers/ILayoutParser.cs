using Botticelli.Framework.Controls.Layouts;

namespace Botticelli.Framework.Controls.Parsers;

public interface ILayoutParser
{
    public ILayout Parse(string jsonText);
    public ILayout ParseFromFile(string jsonFile);
}