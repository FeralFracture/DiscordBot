using DiscordBot.Biz.Interfaces;
using DiscordBot.Objects.Interfaces.IRepositories;
using System.Linq.Expressions;

namespace DiscordBot.Biz
{
    public class GenericBiz<TEntity, TModel> : IGenericBiz<TEntity, TModel>
        where TEntity : class
        where TModel : class
    {
        protected readonly IRepositoryBase<TEntity, TModel> _repository;

        public GenericBiz(IRepositoryBase<TEntity, TModel> repository)
        {
            _repository = repository;
        }
        public virtual IEnumerable<TModel> GetAll()
        {
            return _repository.GetAll();
        }
        public virtual TModel? Get(Guid id)
        {
            return _repository.GetByGUID(id);
        }
        public virtual void Upsert(TModel model, Expression<Func<TEntity, bool>>? matchPredicate = null) => _repository.Upsert(model, matchPredicate);
        public virtual void Delete(TModel model) => _repository.Delete(model);
        public virtual void Delete(Guid id) => _repository.Delete(id);
    }
}
