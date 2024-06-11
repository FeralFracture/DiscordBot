using DiscordBot.Biz.Interfaces;
using DiscordBot.Dal.Entities;
using DiscordBot.Objects.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DiscordBot.Biz.Bizes
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

        public void DeleteByDiscordID(ulong discordId)
        {
            var deleter = _repository.GetByUlongId(discordId);
            if (deleter != null)

                _repository.Delete(deleter);
        }

        public virtual TModel? GetByUlongId(ulong discordId)
        {
            return _repository.GetByUlongId(discordId);
        }

        public virtual void InitialzeServerCheck(TModel model)
        {
            // Get the type of the model
            Type modelType = model.GetType();

            // Find the property that contains "discord" and "id" in its name (case-insensitive)
            var propertyInfo = modelType.GetProperties()
                .FirstOrDefault(p => p.Name.StartsWith("discord", StringComparison.OrdinalIgnoreCase)
                                  && p.Name.EndsWith("id", StringComparison.OrdinalIgnoreCase));

            if (propertyInfo == null)
            {
                throw new InvalidOperationException("No property containing 'discord' and 'id' found on the model.");
            }

            // Ensure the property type is ulong
            if (propertyInfo.PropertyType != typeof(ulong))
            {
                throw new InvalidOperationException("The property found is not of type ulong.");
            }

            // Check if the repository contains an entry with this discordId
            if (_repository.GetByUlongId((ulong)propertyInfo.GetValue(model)!) != null)
            {
                return;
            }

            // Upsert the model
            _repository.Upsert(model);
        }


        public virtual void Prune(IEnumerable<ulong> existingIds)
        {
            // Get the type of the entries in the repository
            Type entryType = typeof(TModel);

            // Find the property that starts with "discord" and ends with "id" in its name (case-insensitive)
            var propertyInfo = entryType.GetProperties()
                .FirstOrDefault(p => p.Name.StartsWith("discord", StringComparison.OrdinalIgnoreCase)
                                  && p.Name.EndsWith("id", StringComparison.OrdinalIgnoreCase));

            if (propertyInfo == null)
            {
                throw new InvalidOperationException("No property starting with 'discord' and ending with 'id' found on the entity.");
            }

            // Ensure the property type is ulong
            if (propertyInfo.PropertyType != typeof(ulong))
            {
                throw new InvalidOperationException("The property found is not of type ulong.");
            }

            var allEntries = _repository.GetAll();
            if (allEntries == null)
            {
                // Handle the case when GetAll returns null
                return;
            }

            foreach (var entry in allEntries)
            {
                var propertyValue = propertyInfo.GetValue(entry);
                if (propertyValue != null && !existingIds.Contains((ulong)propertyValue))
                {
                    _repository.Delete(entry);
                }
            }

        }


    }
}
