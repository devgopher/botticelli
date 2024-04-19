namespace Botticelli.Framework.Controls.Layouts;

public interface ILayoutParser
{
    public ILayout ParseJson(string jsonText);
}