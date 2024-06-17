using DiscordBot.Biz.Interfaces;
using DiscordBot.Dal.Entities;
using DiscordBot.Objects.Interfaces.IRepositories;
using DiscordBot.Objects.Models;
using Microsoft.IdentityModel.Tokens;

namespace DiscordBot.Biz.Bizes
{
    public class ServerBiz : GenericDiscordBiz<Server, ServerModel>, IGenericDiscordBiz<Server, ServerModel>
    {
        private readonly IRoleBiz _roleBiz;
        private readonly ICustomRolesBiz _customRolesBiz;
        public ServerBiz(IDiscordObjectRepositoryBase<Server, ServerModel> repository, IRoleBiz roleBiz, ICustomRolesBiz customRolesBiz) : base(repository)
        {
            _roleBiz = roleBiz;
            _customRolesBiz = customRolesBiz;
        }

        public override void Prune(IEnumerable<ulong> existingIds)
        {
            var deleted = _repository.SelectBy(x => !existingIds.Contains(x.DiscordServerId)).ToList();
            if (deleted.IsNullOrEmpty()) { return; }

            foreach (var entry in deleted)
            {
                _customRolesBiz.DeleteAllByGuildDiscordId(entry.DiscordServerId);
                _roleBiz.DeleteAllByGuildDiscordId(entry.DiscordServerId);
                _repository.Delete(entry);
            }
        }
    }
}
