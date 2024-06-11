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
using Microsoft.EntityFrameworkCore.Design;
using DiscordBot.Dal;
using DiscordBot.SlashCommands;
using DiscordBot;
using DiscordBot.Biz.Bizes;

namespace discordbot
{
    public class Program
    {
        internal static DiscordClient? Client { get; private set; }
        internal static IServiceProvider? ServiceProvider { get; private set; }
        private static CommandsNextExtension? Commands { get; set; }
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

            var commandsConfig = new CommandsNextConfiguration()
            {
                StringPrefixes = new string[] { Configuration.getDPrefix()! },
                EnableMentionPrefix = true,
                EnableDms = true,
                EnableDefaultHelp = false,
                Services = ServiceProvider
            };

            Commands = Client.UseCommandsNext(commandsConfig);
            var slashCommandsConfig = Client.UseSlashCommands(new SlashCommandsConfiguration
            {
                Services = ServiceProvider
            });

            var eventHandlers = ServiceProvider.GetRequiredService<EventHandlers>();
            Client.Ready += eventHandlers.Client_Ready;
            Client.GuildDownloadCompleted += eventHandlers.Guilds_Downloaded;
            Client.GuildDeleted += eventHandlers.Client_GuildDeleted;

            slashCommandsConfig.SlashCommandErrored += AttributesHandler.CmdErroredHandler;

            try
            {
                Commands.RegisterCommands<TestCommand>();

                slashCommandsConfig.RegisterCommands<EntryCommand>(607802183710277648);
                slashCommandsConfig.RegisterCommands<RoleCommands>(607802183710277648);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

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

                   //Add Handlers
                   services.AddScoped<EventHandlers>();

                   // Register ArtEntryRepository and other BIZ/DAL services
                   services.AddScoped<IRepositoryBase<ArtEntry, ArtEntryModel>, ArtEntryRepository>();
                   services.AddScoped<IArtEntryBiz, ArtEntryBiz>();

                   services.AddScoped<IDiscordObjectRepositoryBase<Server, ServerModel>, GenericDiscordObjectRepository<Server, ServerModel>>();
                   services.AddScoped<IGenericDiscordBiz<Server, ServerModel>, ServerBiz>();

                   services.AddScoped<IDiscordObjectRepositoryBase<Role, RoleModel>, GenericDiscordObjectRepository<Role, RoleModel>>();
                   services.AddScoped<IRoleBiz, RoleBiz>();

                   //custom attribute tags
                   services.AddSingleton<RequirePermissionRoleAttribute>();
               });



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


