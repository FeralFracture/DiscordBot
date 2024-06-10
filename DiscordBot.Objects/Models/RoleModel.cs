using System.ComponentModel.DataAnnotations;

namespace DiscordBot.Objects.Models
{
    public record RoleModel
    {
        public Guid? RoleId { get; set; }
        public ulong DiscordRoleId { get; set; }
        public ulong ParentDiscordServerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int permission_level { get; set; } = 0;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public ServerModel? Server { get; set; }
    }
}