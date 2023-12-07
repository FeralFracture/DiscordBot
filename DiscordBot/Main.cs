using DSharpPlus;
using DSharpPlus.CommandsNext;
using Microsoft.Extensions.Configuration;
using discordbot.config;
using discordbot.Commands;

namespace discordbot
{
    internal class Program
    {
        private static DiscordClient? Client { get; set; }
        private static CommandsNextExtension? Commands { get; set; }
        static async Task Main(string[] args)
        {
            var discordConfig = new DiscordConfiguration()
            {
                Intents = DiscordIntents.All,
                Token = Configuration.getDToken(),
                TokenType = TokenType.Bot,
                AutoReconnect = true
            };
            Client = new DiscordClient(discordConfig);
            Client.Ready += Client_Ready;


            var commandsConfig = new CommandsNextConfiguration()
            {
                StringPrefixes = new string[] { Configuration.getDPrefix()! },
                EnableMentionPrefix = true,
                EnableDms = true,
                EnableDefaultHelp = false
            };

            Commands = Client.UseCommandsNext(commandsConfig);

            Commands.RegisterCommands<TestCommand>();

            await Client.ConnectAsync();
            await Task.Delay(-1);
        }

        private static Task Client_Ready(DiscordClient sender, DSharpPlus.EventArgs.ReadyEventArgs args)
        {
            return Task.CompletedTask;
        }
    }
}
