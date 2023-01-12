using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Botticelli.Shared.API.Admin.Responses;

namespace Botticelli.Server.Data.Entities
{
    [Table("BotInfo")]
    public class BotInfo
    {
        [Key]
        public string BotId { get; set; }
        public string? BotName { get; set; }
        public DateTime? LastKeepAlive { get; set; }
        public BotStatus? Status { get; set; }
    }
}
