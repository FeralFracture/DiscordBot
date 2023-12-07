using discordbot.config;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace discordbot.dal
{
    public class BotDbContextFactory : IDesignTimeDbContextFactory<BotDbContext>
    {
        public BotDbContext CreateDbContext(string[] args)
        {
            // Retrieve the connection string from the configuration
            string connectionString = Configuration.getDBConnectionString()!;

            // Set up DbContextOptions
            var optionsBuilder = new DbContextOptionsBuilder<BotDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            // Create the DbContext instance
            return new BotDbContext(optionsBuilder.Options);
        }
    }
}
