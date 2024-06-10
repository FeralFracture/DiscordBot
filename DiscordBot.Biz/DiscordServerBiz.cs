using discordbot.dal.Entities;
using DiscordBot.Biz.Interfaces;
using DiscordBot.Objects.Interfaces.IRepositories;
using DiscordBot.Objects.Models;
using DSharpPlus.Entities;

namespace DiscordBot.Biz
{
    public class DiscordServerBiz : GenericDiscordBiz<Server, ServerModel>, IDiscordServerBiz
    {
        public DiscordServerBiz(IDiscordObjectRepositoryBase<Server, ServerModel> repository) : base(repository)
        {
        }

        public void InitialzeServerCheck(ServerModel server)
        {
            if(_repository.GetByUlongId(server.DiscordServerId) != null)
            {
                return;
            }
            _repository.Upsert(server);
        }

        public void Prune(IEnumerable<DiscordGuild> guilds)
        {
            foreach(var entry in _repository.GetAll())
            {
                if(!guilds.Any(x => x.Id == entry.DiscordServerId))
                {
                    _repository.Delete(entry);
                }
            }
        }
    }
}
