using discordbot.config;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace discordbot.dal
{
    public class DBAccess
    {
        private  BotDbContext db = new BotDbContext(new DbContextOptionsBuilder<BotDbContext>()
             .UseSqlServer(Configuration.getDBConnectionString()) // Assuming you're using SQL Server
            .Options);
        public void Upload(ArtEntryModel entry)
        {
            using (var context = db)
            {
                context.ArtEntries.Add(new ArtEntry()
                {
                    Description = entry.Description,
                    Hours = entry.Hours,
                    Minutes = entry.Minutes,
                    Title = entry.Title,
                    UserId = entry.UserId,
                });
                context.SaveChanges();
            }
        }

        public List<ArtEntryModel> GetForMonth(ulong id, DateTime date)
        {
            using (var context = db)
            {
                var entrylist = context.ArtEntries
                    .Where(e => e.UserId == id)
                    .Where(e => e.date.Month == date.Month)
                    .Where(e => e.date.Year == date.Year)
                    .ToList();
                var outputList = new List<ArtEntryModel>();
                foreach (var entry in entrylist)
                {
                    outputList.Add(entry.ToModel());
                }
                return outputList;
            }
        }
    }
}
