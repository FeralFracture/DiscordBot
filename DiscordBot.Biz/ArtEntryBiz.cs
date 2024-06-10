using DiscordBot.Dal.Entities;
using DiscordBot.Biz.Interfaces;
using DiscordBot.Objects.Interfaces.IRepositories;
using DiscordBot.Objects.Models;

namespace DiscordBot.Biz
{
    public class ArtEntryBiz : GenericBiz<ArtEntry, ArtEntryModel>, IArtEntryBiz
    {
        public ArtEntryBiz(IRepositoryBase<ArtEntry, ArtEntryModel> repository) : base(repository)
        {
        }

        public List<ArtEntryModel> GetForMonth(ulong id, DateTime date)
        {
            return _repository.SelectBy(e =>
            e.UserId == id &&
            e.date.Month == date.Month &&
            e.date.Year == date.Year)
                .ToList();
        }
    }
}
