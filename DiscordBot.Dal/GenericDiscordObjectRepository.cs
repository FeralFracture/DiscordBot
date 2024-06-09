using AutoMapper;
using discordbot.dal;
using DiscordBot.Objects.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Dal
{
    public abstract class GenericDiscordObjectRepository<TEntity, TModel> : GenericRepository<TEntity, TModel>, IDiscordObjectRepositoryBase<TEntity, TModel>
        where TModel : class
        where TEntity : class
    {
        public GenericDiscordObjectRepository(BotDbContext context, IMapper mapper, ILogger<IRepositoryBase<TEntity, TModel>> logger) : base(context, mapper, logger)
        {
        }

        public virtual TModel? GetByUlongId(ulong discordId)
        {
            var ulongProperty = typeof(TEntity)
                .GetProperties()
                .FirstOrDefault(prop => prop.PropertyType == typeof(ulong) && prop.Name.ToLower().Contains("id"));

            if (ulongProperty == null)
            {
                throw new InvalidOperationException("No property of type ulong with 'id' in its name found in the entity.");
            }

            // Build the expression to match the ulong property with the provided value
            var parameter = Expression.Parameter(typeof(TEntity), "x");
            var propertyAccess = Expression.Property(parameter, ulongProperty);
            var equals = Expression.Equal(propertyAccess, Expression.Constant(discordId));
            var lambda = Expression.Lambda<Func<TEntity, bool>>(equals, parameter);

            // Query the database with the generated expression
            var entity = _context.Set<TEntity>().FirstOrDefault(lambda);

            return entity == null ? null : _mapper.Map<TModel>(entity);
        }
    }
}
