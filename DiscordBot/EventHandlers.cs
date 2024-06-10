using discordbot;
using DiscordBot.Biz.Interfaces;
using DiscordBot.Dal.Entities;
using DiscordBot.Objects.Models;
using DSharpPlus;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    public static class EventHandlers
    {
        private static DiscordClient? Client => Program.Client;
        private static IServiceProvider? ServiceProvider => Program.ServiceProvider;
        internal static Task Client_Ready(DiscordClient sender, DSharpPlus.EventArgs.ReadyEventArgs args)
        {
            return Task.CompletedTask;
        }
        internal static Task Guilds_Downloaded(DiscordClient sender, DSharpPlus.EventArgs.GuildDownloadCompletedEventArgs args)
        {
            using (var scope = ServiceProvider!.CreateScope())
            {
                var serverBiz = scope.ServiceProvider.GetRequiredService<IGenericDiscordBiz<Server, ServerModel>>();

                foreach (var server in Client!.Guilds.Values)
                {

                    try
                    {
                        var serverModel = new ServerModel
                        {
                            DiscordServerId = server.Id,
                            Name = server.Name,
                            JoinedAt = DateTime.Now,
                        };
                        serverBiz.InitialzeServerCheck(serverModel);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred while fetching guild details for {server.Id}: {ex.Message}");
                    }
                }
                serverBiz.Prune(Client!.Guilds.Values.Select(guild => guild.Id));
                return Task.CompletedTask;
            }
        }
    }
}
