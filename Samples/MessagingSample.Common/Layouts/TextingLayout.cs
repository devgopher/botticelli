using Botticelli.Framework.Controls.BasicControls;
using Botticelli.Framework.Controls.Layouts;

namespace MessagingSample.Common.Layouts;

public class TextingLayout : BaseLayout
{
    public TextingLayout()
    {
        var row1 = new Row();
        var row2 = new Row();
        
        var item1 = new Item
        {
            Control = new Button
            {
                Content = "/start"
            }
        };
        
        var item2 = new Item
        {
            Control = new Button
            {
                Content = "/info"
            }
        };
        
        var item3 = new Item
        {
            Control = new Button
            {
                Content = "/stop"
            }
        };
        
        row1.AddItem(item1);
        row1.AddItem(item2);
        row2.AddItem(item3);
        
        AddRow(row1);
        AddRow(row2);
    }
}