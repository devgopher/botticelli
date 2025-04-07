namespace Botticelli.Shared.ValueObjects;

public class InvoiceBaseAttachment : BaseAttachment
{
    public InvoiceBaseAttachment(string uid,
                                 string name,
                                 string url,
                                 string title,
                                 string description,
                                 string startParameter,
                                 string currency,
                                 int totalAmount) : base(uid)
    {
        Name = name;
        Url = url;
        Title = title;
        Description = description;
        StartParameter = startParameter;
        Currency = currency;
        TotalAmount = totalAmount;
    }

    public string Url { get; }

    /// <summary>
    ///     Invoice title
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    ///     Description
    /// </summary>
    public string Description { get; set; } = null!;

    /// <summary>
    ///     Unique bot deep-linking parameter that can be used to generate this invoice
    /// </summary>
    public string StartParameter { get; set; } = null!;

    /// <summary>
    ///     Currency: USB, EUR, CNY...
    /// </summary>
    public string Currency { get; set; } = null!;

    /// <summary>
    ///     Total amount like this: 11.95 $
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    ///     Attachment name
    /// </summary>
    public virtual string Name { get; }
}