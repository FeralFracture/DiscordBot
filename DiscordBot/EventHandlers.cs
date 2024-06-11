using discordbot;
using DiscordBot.Biz.Interfaces;
using DiscordBot.Dal.Entities;
using DiscordBot.Objects.Models;
using DSharpPlus;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot
{
    internal class EventHandlers
    {
        private readonly DiscordClient _client;
        private readonly ILogger<EventHandlers> _logger;
        private readonly IGenericDiscordBiz<Server, ServerModel> _serverBiz;

        internal EventHandlers(DiscordClient client, ILogger<EventHandlers> logger, IGenericDiscordBiz<Server, ServerModel> serverBiz)
        {
            _client = client;
            _logger = logger;
            _serverBiz = serverBiz;
        }

        internal Task Client_Ready(DiscordClient _, ReadyEventArgs args)
        {
            _logger.LogInformation("Client is ready");
            return Task.CompletedTask;
        }

        internal Task Guilds_Downloaded(DiscordClient _, GuildDownloadCompletedEventArgs args)
        {
            foreach (var server in _client.Guilds.Values)
            {
                try
                {
                    var serverModel = new ServerModel
                    {
                        DiscordServerId = server.Id,
                        Name = server.Name,
                        JoinedAt = DateTime.Now,
                    };
                    _serverBiz.InitialzeServerCheck(serverModel);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"An error occurred while fetching guild details for {server.Id}");
                }
            }
            _serverBiz.Prune(_client.Guilds.Values.Select(guild => guild.Id));
            return Task.CompletedTask;
        }

        internal Task Client_GuildDeleted(DiscordClient _, GuildDeleteEventArgs args)
        {
            _serverBiz.DeleteByDiscordID(args.Guild.Id);
            _logger.LogInformation($"Guild deleted: {args.Guild.Id}");
            return Task.CompletedTask;
        }
        internal Task Client_GuildCreated(DiscordClient _, GuildCreateEventArgs args)
        {
            var server = args.Guild;
            try
            {
                var serverModel = new ServerModel
                {
                    DiscordServerId = server.Id,
                    Name = server.Name,
                    JoinedAt = DateTime.Now,
                };
                _serverBiz.Upsert(serverModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while adding guild details for {server.Id}");
            }
            _logger.LogInformation($"Guild added: {args.Guild.Id}");
            return Task.CompletedTask;
        }
    }
}
