using System.Threading.Tasks;
using DryIoc;
using DryIoc.Microsoft.DependencyInjection;
using DSharpPlus;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Neruko.Server.Settings;

namespace Neruko.Server
{
    public class Program
    {
        private static DiscordClient _neruko;

        public static async Task Main(string[] args)
        {
            _neruko = await ConfigureDiscord(GetConfigurationRoot());
            await CreateHostBuilder(args).Build().RunAsync();
        }

        /// <summary>
        /// Configures and starts the discord runtime
        /// </summary>
        /// <returns></returns>
        public static async Task<DiscordClient> ConfigureDiscord(IConfigurationRoot configuration)
        {
            var discordSettings = configuration.GetSection("Discord")
                .Get<DiscordSettings>();
            var discord = new DiscordClient(new DiscordConfiguration
            {
                Token = discordSettings.Token,
                TokenType = TokenType.Bot
            });
            await discord.ConnectAsync();
            return discord;
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseServiceProviderFactory(new DryIocAdapter.DryIocServiceProviderFactory())
                .ConfigureContainer<Container>(CompositeRoot);

        /// <summary>
        /// Registers all required services here
        /// </summary>
        /// <param name="hostContext"></param>
        /// <param name="container"></param>
        private static void CompositeRoot(HostBuilderContext hostContext, Container container)
        {
            container.RegisterInstance(_neruko);
        }

        /// <summary>
        /// Gets your configurations
        /// </summary>
        /// <returns></returns>
        private static IConfigurationRoot GetConfigurationRoot()
        {
            return new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();
        }
    }
}
