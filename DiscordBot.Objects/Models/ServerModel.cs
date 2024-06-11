using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Objects.Models
{
    public record ServerModel
    {
        public Guid? ServerId { get; set; }
        public ulong DiscordServerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime JoinedAt { get; set; }
        public DateTime? LeftAt { get; set; }
        public ICollection<RoleModel> Roles { get; set; } = new List<RoleModel>();
        public ICollection<CustomRoleModel> CustomRoles { get; set; } = new List<CustomRoleModel>();
    }
}
