using Botticelli.Framework.Controls.BasicControls;
using Botticelli.Framework.Controls.Exceptions;
using Botticelli.Framework.Controls.Extensions;
using Botticelli.Framework.Controls.Layouts;
using Botticelli.Framework.Vk.Messages.API.Markups;
using Action = Botticelli.Framework.Vk.Messages.API.Markups.Action;

namespace Botticelli.Framework.Vk.Messages.Layout;

public class VkLayoutSupplier : IVkLayoutSupplier
{
    public VkKeyboardMarkup GetMarkup(ILayout layout)
    {
        if (layout == default)
            throw new LayoutException("Layout = null!");

        var buttons = new List<List<VkItem>>(10);

        foreach (var layoutRow in layout.Rows)
        {
            var keyboardElement = new List<VkItem>();

            keyboardElement.AddRange(layoutRow.Items.Where(i => i.Control != default)
                .Select(item =>
            {
                var controlParams = item.Control.Specials.ContainsKey("VK") ? item.Control?.Specials["VK"] : new Dictionary<string, object>();
                
                var action = new Action
                {
                    Type = item.Control is TextButton ? "text" : "button",
                    Payload = "{\"button\": \"" + layout.Rows.IndexOf(layoutRow) + "\"}",
                    Label = item.Control.Content,
                    AppId = controlParams.ReturnValueOrDefault<int>("AppId"),
                    OwnerId = controlParams.ReturnValueOrDefault<int>("OwnerId"),
                    Hash = controlParams.ReturnValueOrDefault<string>("Hash")
                };

                return new VkItem
                {
                    Action = action,
                    Color = controlParams.ReturnValueOrDefault<string>("Color")
                };
            }));
            
            buttons.Add(keyboardElement);
        }

        var markup = new VkKeyboardMarkup()
        {
            OneTime = true,
            Buttons = buttons
        };

        return markup;
    }
}