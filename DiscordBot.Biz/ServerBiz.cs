using DiscordBot.Biz.Interfaces;
using DiscordBot.Dal.Entities;
using DiscordBot.Objects.Interfaces.IRepositories;
using DiscordBot.Objects.Models;
using Microsoft.IdentityModel.Tokens;

namespace DiscordBot.Biz
{
    public class ServerBiz : GenericDiscordBiz<Server, ServerModel>, IGenericDiscordBiz<Server, ServerModel>
    {
        private readonly IGenericDiscordBiz<Role, RoleModel> _roleBiz;
        public ServerBiz(IDiscordObjectRepositoryBase<Server, ServerModel> repository, IGenericDiscordBiz<Role, RoleModel> roleBiz) : base(repository)
        {
            _roleBiz = roleBiz;
        }

        public override void Prune(IEnumerable<ulong> existingIds)
        {
            var deleted = _repository.SelectBy(x => !existingIds.Contains(x.DiscordServerId)).ToList();
            if (deleted.IsNullOrEmpty()) { return; }

            foreach (var entry in deleted)
            {
                _roleBiz.DeleteAllByDiscordID(entry.DiscordServerId);
                _repository.Delete(entry);
            }
        }
    }
}
