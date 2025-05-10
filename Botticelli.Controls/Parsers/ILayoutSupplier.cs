using Botticelli.Controls.Layouts;

namespace Botticelli.Controls.Parsers;

/// <summary>
///     Supplier is responsible for conversion of Layout into messenger-specific controls (for example, ReplyMarkup in
///     Telegram)
/// </summary>
public interface ILayoutSupplier<out TReplyOptions>
{
    public TReplyOptions GetMarkup(ILayout layout);
}