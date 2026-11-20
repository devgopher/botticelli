using Botticelli.Controls.BasicControls;
using Botticelli.Controls.Layouts;
using Botticelli.Controls.Layouts.Inlines;
using FluentAssertions;
using NUnit.Framework;

namespace Botticelli.Controls.Layouts.Tests.Inlines;

[TestFixture]
public class TableTests
{
    [Test]
    public void AddItem_Should_CreateRowsByColumnWidth()
    {
        var table = new Table(2);

        table.AddItem(new Item { Control = new Button("1") });
        table.AddItem(new Item { Control = new Button("2") });
        table.AddItem(new Item { Control = new Button("3") });

        table.Rows.Should().HaveCount(2);
        table.Rows![0].Items.Should().HaveCount(2);
        table.Rows[1].Items.Should().HaveCount(1);
    }

    [Test]
    public void AddRow_Should_AppendCustomRow()
    {
        var table = new Table(3);
        var row = new Row();
        row.AddItem(new Item { Control = new Button("x") });

        table.AddRow(row);

        table.Rows.Should().ContainSingle().Which.Should().BeSameAs(row);
    }
}
