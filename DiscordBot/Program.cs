using DSharpPlus;
using DSharpPlus.CommandsNext;
using Microsoft.Extensions.Configuration;
using discordbot.Commands;
using DSharpPlus.SlashCommands;
using discordbot.SlashCommands;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using DiscordBot.Objects;
using Microsoft.Extensions.Logging;
using DiscordBot.Dal.Repositories;
using DiscordBot.Dal.Entities;
using DiscordBot.Objects.Models;
using DiscordBot.Objects.Interfaces.IRepositories;
using DiscordBot.Biz.Interfaces;
using DiscordBot.Biz;
using Microsoft.EntityFrameworkCore.Design;
using DiscordBot.Dal;

namespace discordbot
{
    public class Program
    {
        private static DiscordClient? Client { get; set; }
        private static CommandsNextExtension? Commands { get; set; }
        private static IServiceProvider? ServiceProvider { get; set; }
        static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            ServiceProvider = host.Services;

            var discordConfig = new DiscordConfiguration()
            {
                Intents = DiscordIntents.All,
                Token = Configuration.getDToken(),
                TokenType = TokenType.Bot,
                AutoReconnect = true
            };
            Client = new DiscordClient(discordConfig);
            Client.Ready += Client_Ready;
            Client.GuildDownloadCompleted += Guilds_Downloaded;
            
            var commandsConfig = new CommandsNextConfiguration()
            {
                StringPrefixes = new string[] { Configuration.getDPrefix()! },
                EnableMentionPrefix = true,
                EnableDms = true,
                EnableDefaultHelp = false,
                Services = ServiceProvider
            };

            Commands = Client.UseCommandsNext(commandsConfig);
            Commands.RegisterCommands<TestCommand>();

            var slashCommandsConfig = Client.UseSlashCommands(new SlashCommandsConfiguration
            {
                Services = ServiceProvider
            });
            slashCommandsConfig.RegisterCommands<EntryCommand>();

            await Client.ConnectAsync();
            await Task.Delay(-1);
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                Configuration.Initialize();
            })
            .ConfigureServices((context, services) =>
               {
                   services.AddLogging(builder =>
                   {
                       builder.AddFilter("discordbot", LogLevel.Information); // Adjust as needed
                       builder.AddConsole();
                   });
                   // Configure Entity Framework Core using factory
                   services.AddDbContext<BotDbContext>(options => options.UseSqlServer(Configuration.getDBConnectionString()));
                   services.AddSingleton<IDesignTimeDbContextFactory<BotDbContext>, BotDbContextFactory>();

                   // Add AutoMapper
                   services.AddAutoMapper(typeof(AutoMapperProfile));

                   // Register ArtEntryRepository and other BIZ/DAL services
                   services.AddScoped<IRepositoryBase<ArtEntry, ArtEntryModel>, ArtEntryRepository>();
                   services.AddScoped<IArtEntryBiz, ArtEntryBiz>();

                   services.AddScoped<IDiscordObjectRepositoryBase<Server, ServerModel>, GenericDiscordObjectRepository<Server, ServerModel>>();
                   services.AddScoped<IGenericDiscordBiz<Server, ServerModel>, ServerBiz>();

                   services.AddScoped<IDiscordObjectRepositoryBase<Role, RoleModel>, GenericDiscordObjectRepository<Role, RoleModel>>();
                   services.AddScoped<IGenericDiscordBiz<Role, RoleModel>, GenericDiscordBiz<Role, RoleModel>>();
               });
        private static Task Client_Ready(DiscordClient sender, DSharpPlus.EventArgs.ReadyEventArgs args)
        {
            return Task.CompletedTask;
        }
        private static async Task Guilds_Downloaded(DiscordClient sender, DSharpPlus.EventArgs.GuildDownloadCompletedEventArgs args)
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
            }
        }
        
        #region Configuration
        private static class Configuration
        {
            private static IConfigurationRoot? config;
            private static bool initialized = false;
            public static void Initialize(bool dbMigration = false)
            {
                initialized = true;
                config = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appsettings.Development.json", optional: false)
                        .Build();
            }

            public static string? getDToken()
            {
                if (!initialized)
                {
                    Initialize();
                }
                return config!["DiscordBotConfigs:Token"];
            }
            public static string? getDPrefix()
            {
                if (!initialized)
                {
                    Initialize();
                }
                return config!["DiscordBotConfigs:Prefix"];
            }
            public static string? getDBConnectionString(bool dbMigration = false)
            {
                if (!initialized)
                {
                    Initialize(dbMigration);
                }
                return config!["ConnectionStrings:DBConnectionString"];
            }
        }
        #endregion

        #region EFC Factory
        private class BotDbContextFactory : IDesignTimeDbContextFactory<BotDbContext>
        {
            public BotDbContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<BotDbContext>();
                optionsBuilder.UseSqlServer(Configuration.getDBConnectionString());

                return new BotDbContext(optionsBuilder.Options);
            }
        }
        #endregion
    }
}


