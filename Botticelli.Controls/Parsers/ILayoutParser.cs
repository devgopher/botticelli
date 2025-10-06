using Botticelli.Controls.Layouts;

namespace Botticelli.Controls.Parsers;

public interface ILayoutParser
{
    public ILayout Parse(string jsonText);
    public ILayout ParseFromFile(string jsonFile);
}