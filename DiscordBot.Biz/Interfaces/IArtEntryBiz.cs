using discordbot.dal.Entities;
using DiscordBot.Objects.Models;

namespace DiscordBot.Biz.Interfaces
{
    public interface IArtEntryBiz : IGenericBiz<ArtEntry, ArtEntryModel>
    {
        List<ArtEntryModel> GetForMonth(ulong id, DateTime date);
    }
}
