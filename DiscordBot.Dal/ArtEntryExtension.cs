using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace discordbot.dal
{
    public static class ArtEntryExtension
    {
        public static ArtEntryModel ToModel(this ArtEntry entry)
        {
            return new ArtEntryModel()
            {
                UserId = entry.UserId,
                ArtEntryId = entry.ArtEntryId,
                date = entry.date,
                Description = entry.Description,
                Hours = entry.Hours,
                Minutes = entry.Minutes,
                Title = entry.Title
            };
        }
    }
}
