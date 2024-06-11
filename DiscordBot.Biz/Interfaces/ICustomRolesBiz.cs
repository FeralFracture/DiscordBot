using DiscordBot.Dal.Entities;
using DiscordBot.Objects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Biz.Interfaces
{
    public interface ICustomRolesBiz : IGenericDiscordBiz<CustomRole, CustomRoleModel>
    {
        void DeleteAllByGuildDiscordId(ulong guildId);
        bool HasMaxRolesForGuild(ulong guildId, ulong userId);
    }
}
