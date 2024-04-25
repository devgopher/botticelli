using System.Text.Json.Serialization;

namespace Botticelli.Shared.ValueObjects;

[JsonDerivedType(typeof(BaseAttachment), typeDiscriminator: "base")]
[JsonPolymorphic(UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToBaseType)]
[JsonDerivedType(typeof(BinaryBaseAttachment), typeDiscriminator: "binaryAttachment")]
[JsonDerivedType(typeof(InvoiceBaseAttachment), typeDiscriminator: "invoiceAttachment")]
public class BaseAttachment
{
    public virtual string Uid { get; }
    public virtual string Name { get; }
    public virtual string OwnerId { get; }
}