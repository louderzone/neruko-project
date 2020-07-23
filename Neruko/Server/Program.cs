using System;
using System.Threading.Tasks;
using DryIoc;
using DryIoc.Microsoft.DependencyInjection;
using DSharpPlus;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Neruko.Server.Services.Discord;
using Neruko.Server.Settings;

namespace Neruko.Server
{
    public class Program
    {
        #nullable enable
        private static IDiscordService? _discordService;
        #nullable disable

        /// <summary>
        /// Entry point of the program
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static async Task Main(string[] args)
        {
            _discordService = await ConfigureDiscord(GetConfigurationRoot());    
            await CreateHostBuilder(args).Build().RunAsync();
        }

        /// <summary>
        /// Configures and starts the discord runtime
        /// </summary>
        /// <returns></returns>
        public static async Task<IDiscordService> ConfigureDiscord(IConfigurationRoot configuration)
        {
            try
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
                _discordService = new DiscordService(discord);
            }
            catch (InvalidOperationException ex)
            {
                _discordService = new NoDiscordService(ex.Message);
            }
            catch (Exception)
            {
                // FIXME: DSharpPlus is not throwing the correct `UnauthorizedException`
                // Thus just an System.Exception is raised. We have no choice but to catch all Exception
                // If they update later to fix that, please fix this to only capture the related exception.
                _discordService = new NoDiscordService();
            }

            return _discordService;
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
            container.RegisterInstance(_discordService);
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
