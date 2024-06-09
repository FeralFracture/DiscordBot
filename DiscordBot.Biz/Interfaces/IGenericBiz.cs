using System.Linq.Expressions;

namespace DiscordBot.Biz.Interfaces
{
    public interface IGenericBiz<TEntity, TModel>
    {
        IEnumerable<TModel> GetAll();
        TModel? Get(Guid id);
        void Upsert(TModel model, Expression<Func<TEntity, bool>>? matchPredicate = null);
        void Delete(TModel model);
        void Delete(Guid id);
    }
}
