using Botticelli.Framework.Controls.Exceptions;
using Telegram.Bot.Types.ReplyMarkups;

namespace Botticelli.Framework.Controls.Layouts;

public class TelegramLayoutSupplier : ITelegramLayoutSupplier
{
    public ReplyKeyboardMarkup GetOptions(ILayout layout)
    {
        if (layout == default)
            throw new LayoutException("Layout = null!");
     
        var elems = new List<List<KeyboardButton>>(5);

        foreach (var layoutRow in layout.Rows)
        {
            var keyboardElement = new List<KeyboardButton>();
            elems.Add(keyboardElement);

            keyboardElement.AddRange(layoutRow.Items.Select(item => new KeyboardButton(item?.Control?.Content)));
        }
        
        return new ReplyKeyboardMarkup(elems)
        {
            ResizeKeyboard = true
        };
    }
}