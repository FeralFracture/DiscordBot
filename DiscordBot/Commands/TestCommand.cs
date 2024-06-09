using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Microsoft.Extensions.Logging;

namespace discordbot.Commands
{
    public class TestCommand : BaseCommandModule
    {
        private readonly ILogger<TestCommand> _logger;

        public TestCommand(ILogger<TestCommand> logger)
        {
            _logger = logger;   
        }

        [Command("test")]
        public async Task TestCommandMEssage(CommandContext ctx)
        {
            _logger.LogInformation("Test information log");
            _logger.LogWarning("test warning log");
            _logger.LogError("test error log");
            _logger.LogCritical("test critial Log");
            await ctx.Channel.SendMessageAsync($"{ctx.User.Username} said something");
        }
    }
}
