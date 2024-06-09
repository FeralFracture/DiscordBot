using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Objects.Models
{
    public record ServerModel
    {
        public Guid ServerId { get; set; }
        public ulong DiscordServerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime JoinedAt { get; set; }
        public DateTime? LeftAt { get; set; }
    }
}
