using DiscordBot.Biz.Interfaces;
using DiscordBot.Objects.Interfaces.IRepositories;

namespace DiscordBot.Biz
{
    public class GenericDiscordBiz<TEntity, TModel> : GenericBiz<TEntity, TModel>, IGenericDiscordBiz<TEntity, TModel>
        where TEntity : class
        where TModel : class
    {
        protected new readonly IDiscordObjectRepositoryBase<TEntity, TModel> _repository;
        public GenericDiscordBiz(IDiscordObjectRepositoryBase<TEntity, TModel> repository) : base(repository)
        {
            _repository = repository;
        }

        public virtual TModel? GetByUlongId(ulong discordId)
        {
            return _repository.GetByUlongId(discordId);
        }
    }
}
