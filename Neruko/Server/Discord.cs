using System;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using Microsoft.Extensions.Configuration;
using Neruko.Server.Services.Discord;
using Neruko.Server.Services.Discord.Commands;
using Neruko.Server.Settings;

namespace Neruko.Server
{
    public static class Discord
    {
        /// <summary>
        /// Starts the discord service
        /// </summary>
        /// <param name="configuration">Server environment configurations</param>
        /// <returns></returns>
        public static async Task<IDiscordService> CreateService(IConfigurationRoot configuration)
        {
            IDiscordService client;
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
                LoadCommandModules(discord);
                await discord.ConnectAsync();

                client = new DiscordService(discord);
                Console.WriteLine($"Discord bot connected as {discord.CurrentUser.Username}");
            }
            catch (InvalidOperationException ex)
            {
                client = new NoDiscordService(ex.Message);
            }
            catch (Exception)
            {
                // FIXME: DSharpPlus is not throwing the correct `UnauthorizedException`
                // Thus just an System.Exception is raised. We have no choice but to catch all Exception
                // If they update later to fix that, please fix this to only capture the related exception.
                client = new NoDiscordService();
            }

            return client;
        }

        /// <summary>
        /// Loads all `ICommandModules` in the project
        /// </summary>
        /// <param name="client"></param>
        private static void LoadCommandModules(DiscordClient client)
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
    }
}
