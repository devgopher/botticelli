using Botticelli.Framework.Controls.Layouts;

namespace Botticelli.Framework.Controls.Tests.Layouts;

[TestFixture]
[TestOf(typeof(TelegramLayoutSupplier))]
public class TelegramLayoutSupplierTest
{
    private readonly LayoutJsonParser _layoutJsonParser = new();
    private readonly TelegramLayoutSupplier _supplier = new();

    [Test]
    public void GetMarkupTest()
    {
        var jsonText = File.ReadAllText("TestCases/CorrectLayout.json");
        var layout = _layoutJsonParser.ParseJson(jsonText);
        var markup = _supplier.GetMarkup(layout);
        
        Assert.IsNotNull(markup);
    }
}