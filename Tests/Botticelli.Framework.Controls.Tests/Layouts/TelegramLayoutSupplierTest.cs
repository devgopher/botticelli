using Botticelli.Framework.Controls.Layouts;
using Botticelli.Framework.Controls.Parsers;

namespace Botticelli.Framework.Controls.Tests.Layouts;

[TestFixture]
[TestOf(typeof(TelegramLayoutSupplier))]
public class TelegramLayoutSupplierTest
{
    private readonly JsonLayoutParser _jsonLayoutParser = new();
    private readonly ITelegramLayoutSupplier _supplier = new TelegramLayoutSupplier();

    [Test]
    public void GetMarkupTest()
    {
        var jsonText = File.ReadAllText("TestCases/CorrectLayout.json");
        var layout = _jsonLayoutParser.Parse(jsonText);
        var markup = _supplier.GetMarkup(layout);
        
        Assert.IsNotNull(markup);
    }
}