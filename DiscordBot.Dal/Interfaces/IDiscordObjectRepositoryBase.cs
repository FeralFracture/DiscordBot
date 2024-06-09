using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Objects.Interfaces.IRepositories
{
    public interface IDiscordObjectRepositoryBase<TEntity, TModel> : IRepositoryBase<TEntity, TModel>
    {
        TModel? GetByUlongId(ulong discordId);
    }
}
