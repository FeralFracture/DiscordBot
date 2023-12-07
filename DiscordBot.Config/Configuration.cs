using Microsoft.Extensions.Configuration;

namespace discordbot.config
{
    public static class Configuration
    {
        private static IConfigurationRoot? config;
        private static bool initialized = false;
        public static void Initialize()
        {
            initialized = true;
            config = new ConfigurationBuilder()
                    .SetBasePath($"{Directory.GetCurrentDirectory()}/../discordbot.config")
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.Development.json", optional: false)
                    .Build();
        }

        public static string? getDToken()
        {
            if(!initialized)
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
        public static string? getDBConnectionString()
        {
            if (!initialized)
            {
                Initialize();
            }
            return config!["ConnectionStrings:DBConnectionString"];
        }
    }
}
