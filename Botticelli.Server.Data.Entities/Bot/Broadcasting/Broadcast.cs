using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Botticelli.Server.Data.Entities.Bot.Broadcasting;

[Table("Broadcasts")]
public class Broadcast
{
    [Key]
    public required string Id { get; set; }

    public required string BotId { get; set; }
    public required string Body { get; set; }
    public List<BroadcastAttachment>? Attachments { get; set; }
    public DateTime Timestamp { get; set; }
    public bool Sent { get; set; } = false;
    public bool Received { get; set; } = false;
}