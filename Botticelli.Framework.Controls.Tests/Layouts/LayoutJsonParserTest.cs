using Botticelli.Framework.Controls.Exceptions;
using Botticelli.Framework.Controls.Layouts;

namespace Botticelli.Framework.Controls.Tests.Layouts;

[TestFixture]
[TestOf(typeof(LayoutJsonParser))]
public class LayoutJsonParserTest
{
    private readonly LayoutJsonParser _layoutJsonParser = new();

    [Test]
    [TestCase]
    public void ValidParseJsonTest()
    {
        var jsonText = File.ReadAllText("TestCases/CorrectLayout.json");

        var layout = _layoutJsonParser.ParseJson(jsonText);

        Assert.That(layout, Is.Not.Null);
    }

    [Test]
    [TestCase]
    public void InvalidParseJsonTest()
    {
        var jsonText = File.ReadAllText("TestCases/InvalidLayout.json");

        Assert.Throws<LayoutException>(() => _layoutJsonParser.ParseJson(jsonText));
    }
}