using System;
using System.Linq;
using System.Threading.Tasks;
using DryIoc;
using DryIoc.Microsoft.DependencyInjection;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Neruko.Server.Services.Discord;
using Neruko.Server.Services.Discord.Commands;
using Neruko.Server.Settings;

namespace Neruko.Server
{
    public class Program
    {
        #nullable enable
        public static IContainer Container { get; private set; } = new Container();
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
                // Get discord credentials
                var discordSettings = configuration.GetSection("Discord")
                    .Get<DiscordSettings>();
                if(discordSettings?.Token == null) {
                    // Discord application credentials not set
                    throw new InvalidOperationException("Please set your `Discord:Token` secret."
                                                        + " For more info, see https://github.com/louderzone/neruko-project/issues/9");
                }

                // Loads the discord client
                var discord = new DiscordClient(new DiscordConfiguration
                {
                    Token = discordSettings.Token,
                    TokenType = TokenType.Bot
                });
                ConfigureCommands(discord);
                await discord.ConnectAsync();
                _discordService = new DiscordService(discord);
                Console.WriteLine($"Discord bot connected as {discord.CurrentUser.Username}");
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
        /// Loads all `ICommandModules` in the project
        /// </summary>
        /// <param name="client"></param>
        private static void ConfigureCommands(DiscordClient client)
        {
            var config = new CommandsNextConfiguration
            {
                // let's use the string prefix defined in config.json
                StringPrefix = "/",
                // enable responding in direct messages
                EnableDms = false,
                // enable mentioning the bot as a command prefix
                EnableMentionPrefix = true
            };

            var type = typeof(ICommandModule); // Get the type of our interface
            var modules = AppDomain.CurrentDomain.GetAssemblies() // Get the assemblies associated with our project
                .SelectMany(s => s.GetTypes()) // Get all the types
                .Where(p => type.IsAssignableFrom(p) // Filter to find any type that can be assigned to an ICommandModule
                            && !p.IsInterface); // Ignore the interfaces themselves

            var commands = client.UseCommandsNext(config);
            var commandsList = modules as Type[] ?? modules.ToArray(); // Execute the Linq
            foreach (var command in commandsList)
            {
                // Loop through the list and register each command module with CommandsNext
                commands.RegisterCommands(command);
            }
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
            Container = container;
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
