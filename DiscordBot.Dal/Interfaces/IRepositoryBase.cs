using DiscordBot.Objects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Objects.Interfaces.IRepositories
{
    public interface IRepositoryBase<TEntity, TModel>
    {
        IEnumerable<TModel> GetAll();
        bool Contains(Expression<Func<TEntity, bool>> expression);
        TModel? GetByGUID(Guid id);
        IEnumerable<TModel> SelectBy(Expression<Func<TEntity, bool>> expression);
        void Upsert(TModel model, Expression<Func<TEntity, bool>>? matchPredicate = null);
        void Delete(TModel model);
        void Delete(Guid id);
    }
}
