using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace discordbot.Commands
{
    public class TestCommand : BaseCommandModule
    {
        [Command("test")]
        public async Task TestCommandMEssage(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync($"{ctx.User.Username} said something");
        }
    }
}
