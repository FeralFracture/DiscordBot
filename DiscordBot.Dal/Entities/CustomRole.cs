using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscordBot.Dal.Entities
{
    [Table("CustomColorRoles", Schema = "ff")]
    public class CustomRole
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CustomRoleId { get; set; }
        public ulong DiscordRoleId { get; set; }
        public ulong ParentDiscordServerId { get; set; }
        public ulong OwnerUserDiscordId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int[] RoleColor { get; set; } = { 0, 0, 0 };
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public Server? Server { get; set; }
        [Timestamp]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public byte[] RowVersion { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}
