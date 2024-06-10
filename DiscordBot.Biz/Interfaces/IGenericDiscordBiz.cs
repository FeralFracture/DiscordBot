using DiscordBot.Objects.Models;
using DSharpPlus.Entities;

namespace DiscordBot.Biz.Interfaces
{
    public interface IGenericDiscordBiz<TEntity, TModel> : IGenericBiz<TEntity, TModel>
    {
        TModel? GetByUlongId(ulong discordId);
        void InitialzeServerCheck(TModel model);
        void Prune(IEnumerable<ulong> existingIds);
        void DeleteAllByDiscordID(ulong discordId);
    }
}
