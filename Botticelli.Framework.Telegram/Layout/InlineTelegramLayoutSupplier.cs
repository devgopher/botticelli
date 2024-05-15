using Botticelli.Framework.Controls.Exceptions;
using Botticelli.Framework.Controls.Layouts;
using Telegram.Bot.Types.ReplyMarkups;

namespace Botticelli.Framework.Telegram.Layout;

public class InlineTelegramLayoutSupplier : IInlineTelegramLayoutSupplier
{
    public InlineKeyboardMarkup GetMarkup(ILayout layout)
    {
        if (layout == default)
            throw new LayoutException("Layout = null!");
     
        var elems = new List<List<InlineKeyboardButton>>(6);

        foreach (var layoutRow in layout.Rows.Where(row => row != null))
        {
            var keyboardElement = new List<InlineKeyboardButton>();
            keyboardElement.AddRange(layoutRow.Items.Select(item => new InlineKeyboardButton(item.Control?.Content!)
            {
                CallbackData = item.Control!.Params!.ContainsKey("CallbackData") ? item.Control?.Params["CallbackData"]  : "none"
            }));
            
            elems.Add(keyboardElement);
        }
        
        return new(elems);
    }
}