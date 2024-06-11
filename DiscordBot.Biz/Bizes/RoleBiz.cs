using DiscordBot.Biz.Interfaces;
using DiscordBot.Dal.Entities;
using DiscordBot.Objects.Interfaces.IRepositories;
using DiscordBot.Objects.Models;

namespace DiscordBot.Biz.Bizes
{
    public class RoleBiz : GenericDiscordBiz<Role, RoleModel>, IRoleBiz
    {
        public RoleBiz(IDiscordObjectRepositoryBase<Role, RoleModel> repository) : base(repository)
        {
        }

        public void DeleteAllByGuildDiscordId(ulong guildId)
        {
            var deleters = _repository.SelectBy(x => x.ParentDiscordServerId == guildId);
            foreach (var deleter in deleters)
            {
                _repository.Delete(deleter);
            }
        }
    }
}
