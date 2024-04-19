using System.Collections.Immutable;

namespace Botticelli.Framework.Controls.Layouts;

/// <summary>
/// Supplier is responsible for conversion of Layout into messenger-specific controls (soft example, ReplyMarkup in Telegram) 
/// </summary>
public interface ILayoutSupplier<out TReplyOptions>
{
    public TReplyOptions GetMarkup(ILayout layout);
}