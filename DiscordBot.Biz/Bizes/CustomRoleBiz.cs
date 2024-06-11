using DiscordBot.Biz.Interfaces;
using DiscordBot.Dal.Entities;
using DiscordBot.Objects.Interfaces.IRepositories;
using DiscordBot.Objects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Biz.Bizes
{
    public class CustomRoleBiz : GenericDiscordBiz<CustomRole, CustomRoleModel>, ICustomRolesBiz
    {
        public CustomRoleBiz(IDiscordObjectRepositoryBase<CustomRole, CustomRoleModel> repository) : base(repository)
        {
        }

        public void DeleteAllByGuildDiscordId(ulong guildId)
        {
            throw new NotImplementedException();
        }

        public bool HasMaxRolesForGuild(ulong guildId, ulong userId)
        {
            throw new NotImplementedException();
        }
    }
}
