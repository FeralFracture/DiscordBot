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

        public DbSet<Product> Products { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        // Entirely test junk that is not going to remain lol
    }

}
