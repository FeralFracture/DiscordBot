using DiscordBot.Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace DiscordBot.Dal
{
    public class BotDbContext : DbContext
    {
        public BotDbContext(DbContextOptions<BotDbContext> options) : base(options)
        {
        }

        public DbSet<ArtEntry> ArtEntries { get; set; }
        public DbSet<Server> Servers { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>()
                .HasOne(r => r.Server)
                .WithMany(s => s.Roles)
                .HasForeignKey(r => r.ParentDiscordServerId)
                .HasPrincipalKey(s => s.DiscordServerId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
