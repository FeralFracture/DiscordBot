﻿using AutoMapper;
using discordbot.dal;
using DiscordBot.Objects.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace DiscordBot.Dal
{
    public class GenericRepository<TEntity, TModel> : IRepositoryBase<TEntity, TModel>
        where TEntity : class
        where TModel : class
    {
        protected readonly BotDbContext _context;
        protected readonly IMapper _mapper;
        protected readonly ILogger<IRepositoryBase<TEntity, TModel>> _logger;
        public GenericRepository(BotDbContext context, IMapper mapper, ILogger<IRepositoryBase<TEntity, TModel>> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }
        public virtual IEnumerable<TModel> GetAll()
        {
            var entities = _context.Set<TEntity>();
            return _mapper.Map<IEnumerable<TModel>>(entities);
        }
        public bool Contains(Expression<Func<TEntity, bool>> expression)
        {
            return _context.Set<TEntity>().FirstOrDefault(expression) != null;
        }
        public virtual TModel? GetByGUID(Guid id)
        {
            var entity = _context.Set<TEntity>().Find(id);
            return entity == null ? null : _mapper.Map<TModel>(entity);
        }
        public virtual IEnumerable<TModel> SelectBy(Expression<Func<TEntity, bool>> expression)
        {
            var entities = _context.Set<TEntity>().Where(expression);
            return _mapper.Map<IEnumerable<TModel>>(entities);
        }
        public virtual void Upsert(TModel model, Expression<Func<TEntity, bool>>? matchPredicate = null)
        {
            var transaction = _context.Database.CurrentTransaction == null ? _context.Database.BeginTransaction() : null;
            try
            {
                TEntity? entity = null;

                if (matchPredicate != null)
                {
                    entity = _context.Set<TEntity>().FirstOrDefault(matchPredicate);
                }

                if (entity == null)
                {
                    entity = _mapper.Map<TEntity>(model);
                    _context.Set<TEntity>().Add(entity);
                }
                else
                {
                    _mapper.Map(model, entity);
                }

                _context.SaveChanges();
                _context.Database.CommitTransaction();
            }
            catch
            {
                transaction?.Rollback();
                throw;
            }
        }

        public virtual void Delete(TModel? model)
        {
            if (model != null)
            {
                var entity = _mapper.Map<TEntity>(model);
                var primaryKey = GetPrimaryKey(entity);
                var existingEntity = _context.Set<TEntity>().Find(primaryKey);
                if (existingEntity != null)
                {
                    _context.Set<TEntity>().Remove(existingEntity);
                    _context.SaveChanges();
                }
            }
        }

        public virtual void Delete(Guid id)
        {
            var transaction = _context.Database.CurrentTransaction == null ? _context.Database.BeginTransaction() : null;
            try
            {
                var entity = _context.Set<TEntity>().Find(id);
                _context.Set<TEntity>().Remove(entity!);
                _context.SaveChanges();
                _context.Database.CommitTransaction();
            }
            catch
            {
                transaction?.Rollback();
                throw;
            }
        }

        private object GetPrimaryKey(TEntity entity)
        {
            var keyProperties = _context.Model.FindEntityType(typeof(TEntity))!.FindPrimaryKey()!.Properties;
            var primaryKey = new object[keyProperties.Count];
            var i = 0;
            foreach (var keyProperty in keyProperties)
            {
                primaryKey[i++] = entity.GetType().GetProperty(keyProperty.Name)?.GetValue(entity)!;
            }
            return primaryKey[0]; // Assuming single-column primary key
        }


    }
}

