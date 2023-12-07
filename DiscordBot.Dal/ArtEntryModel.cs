using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace discordbot.dal
{
    public record ArtEntryModel
    {
        public Guid ArtEntryId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ulong UserId { get; set; }
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public DateTime date { get; set; }
    }
}
