using DiscordBot.Dal.Entities;
using DiscordBot.Objects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Biz.Interfaces
{
    public interface IRoleBiz : IGenericDiscordBiz<Role, RoleModel>
    {
        void DeleteAllByGuildDiscordId(ulong guildId);
    }
}
