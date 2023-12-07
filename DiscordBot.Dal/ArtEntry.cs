using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace discordbot.dal
{
    [Table("artLog", Schema = "as")]
    public class ArtEntry
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ArtEntryId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ulong UserId { get; set; }
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public DateTime date { get; set; } = DateTime.Now.AddHours(2);
        [Timestamp]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public byte[] RowVersion { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    }
}
