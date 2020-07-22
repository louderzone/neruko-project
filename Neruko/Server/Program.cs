using System;
using System.Threading.Tasks;
using DryIoc;
using DryIoc.Microsoft.DependencyInjection;
using DSharpPlus;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Neruko.Server.Services;
using Neruko.Server.Settings;

namespace Neruko.Server
{
    public class Program
    {
        #nullable enable
        private static DiscordClient? _neruko;
        #nullable disable

        /// <summary>
        /// Entry point of the program
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
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
            if(discordSettings?.Token == null) {
                // Discord application credentials not set
                throw new InvalidOperationException("Please set your `Discord:Token` secret."
                                                    + " For more info, see https://github.com/louderzone/neruko-project/issues/9");
            }

            var discord = new DiscordClient(new DiscordConfiguration
            {
                Token = discordSettings.Token,
                TokenType = TokenType.Bot
            });
            await discord.ConnectAsync();
            return discord;
        }

        /// <summary>
        /// Creates the web server
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
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
            container.Register<IDiscordService, DiscordService>();
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
