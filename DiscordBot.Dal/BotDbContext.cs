using discordbot.config;
using Microsoft.EntityFrameworkCore;

namespace discordbot.dal
{
    public class BotDbContext : DbContext
    {
        private readonly string connectionString;

        public BotDbContext(DbContextOptions<BotDbContext> options) : base(options)
        {
            this.connectionString = Configuration.getDBConnectionString()!;
        }

        public DbSet<ArtEntry> ArtEntries { get; set; }
    }


}
