using DSharpPlus.Entities;

namespace DiscordBot.Objects.Models
{
    public record CustomRoleModel
    {
        public Guid? CustomRoleId { get; set; }
        public ulong DiscordRoleId { get; set; }
        public ulong ParentDiscordServerId { get; set; }
        public ulong OwnerUserDiscordId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DiscordColor RoleColor { get; set; } = new DiscordColor(0,0,0);
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public ServerModel? Server { get; set; }
    }
}
