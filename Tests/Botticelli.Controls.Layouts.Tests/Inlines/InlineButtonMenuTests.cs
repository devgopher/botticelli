using Botticelli.Controls.BasicControls;
using Botticelli.Controls.Layouts;
using Botticelli.Controls.Layouts.Inlines;
using FluentAssertions;
using NUnit.Framework;

namespace Botticelli.Controls.Layouts.Tests.Inlines;

[TestFixture]
public class InlineButtonMenuTests
{
    [Test]
    public void Constructor_Should_RejectNonPositiveDimensions()
    {
        var rowsAct = () => new InlineButtonMenu(0, 2);
        var colsAct = () => new InlineButtonMenu(2, 0);

        rowsAct.Should().Throw<InvalidDataException>().WithMessage("*rows*");
        colsAct.Should().Throw<InvalidDataException>().WithMessage("*columns*");
    }

    [Test]
    public void Header_Should_CreateTextControlInFirstRow()
    {
        var menu = new InlineButtonMenu(2, 2)
        {
            Header = "Title"
        };

        menu.Header.Should().Be("Title");
        menu.Rows.Should().HaveCount(1);
        menu.Rows![0].Items[0].Control.Should().BeOfType<Text>();
        ((Text)menu.Rows[0].Items[0].Control!).Content.Should().Be("Title");
    }

    [Test]
    public void Header_Should_UpdateExistingHeaderItem()
    {
        var menu = new InlineButtonMenu(2, 2)
        {
            Header = "One"
        };
        menu.Header = "Two";

        menu.Header.Should().Be("Two");
        menu.Rows.Should().HaveCount(1);
    }

    [Test]
    public void AddRow_Should_AppendRow()
    {
        var menu = new InlineButtonMenu(2, 2);
        var row = new Row();

        menu.AddRow(row);

        menu.Rows.Should().ContainSingle().Which.Should().BeSameAs(row);
    }

    [Test]
    public void AddControl_Should_FillRowsUntilLimit()
    {
        var menu = new InlineButtonMenu(1, 2);

        menu.AddControl(new Button("a"));
        menu.AddControl(new Button("b"));

        menu.Rows.Should().HaveCount(1);
        menu.Rows![0].Items.Should().HaveCount(2);
        menu.Invoking(m => m.AddControl(new Button("c")))
            .Should().Throw<InvalidOperationException>()
            .WithMessage("*exceeded*");
    }

    [Test]
    public void AddControl_WithHeader_Should_SkipHeaderRowWhenCheckingLimit()
    {
        var menu = new InlineButtonMenu(1, 1)
        {
            Header = "H"
        };
        var contentRow = new Row();
        contentRow.AddItem(new Item { Control = new Button("btn") });
        menu.AddRow(contentRow);

        menu.Rows.Should().HaveCount(2);
        menu.Invoking(m => m.AddControl(new Button("extra")))
            .Should().Throw<InvalidOperationException>()
            .WithMessage("*exceeded*");
    }
}

