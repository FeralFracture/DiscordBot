using DiscordBot.Dal.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Dal.Entities
{
    [Table("Servers", Schema = "ff")]
    public class Server
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ServerId { get; set; }
        public ulong DiscordServerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime JoinedAt { get; set; }
        public DateTime? LeftAt { get; set; }
        public ICollection<Role> Roles { get; set; } = new List<Role>();
        public ICollection<CustomRole> CustomRoles { get; set; } = new List<CustomRole>();
        [Timestamp]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public byte[] RowVersion { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}
