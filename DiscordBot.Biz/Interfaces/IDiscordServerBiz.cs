using discordbot.dal.Entities;
using DiscordBot.Objects.Models;
using DSharpPlus.Entities;

namespace DiscordBot.Biz.Interfaces
{
    public interface IDiscordServerBiz : IGenericDiscordBiz<Server, ServerModel>
    {
        void InitialzeServerCheck(ServerModel server);
        void Prune(IEnumerable<DiscordGuild> guilds);
    }
}
