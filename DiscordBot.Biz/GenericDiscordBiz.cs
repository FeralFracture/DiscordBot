using DiscordBot.Biz.Interfaces;
using DiscordBot.Objects.Interfaces.IRepositories;

namespace DiscordBot.Biz
{
    public class GenericDiscordBiz<TEntity, TModel> : GenericBiz<TEntity, TModel>, IGenericDiscordBiz<TEntity, TModel>
        where TEntity : class
        where TModel : class
    {
        public GenericDiscordBiz(IDiscordObjectRepositoryBase<TEntity, TModel> repository) : base(repository)
        {
        }

        public virtual TModel? GetByUlongId(ulong discordId)
        {
            return ((IDiscordObjectRepositoryBase<TEntity, TModel>)_repository).GetByUlongId(discordId);
        }
    }
}
