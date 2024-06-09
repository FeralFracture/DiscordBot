using discordbot.dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace discordbot.dal
{
    public class BotDbContext : DbContext
    {

        public BotDbContext(DbContextOptions<BotDbContext> options) : base(options)
        {
        }


        public DbSet<ArtEntry> ArtEntries { get; set; }
        //public DbSet<UserProfile> UserProfiles { get; set; }
    }
}
