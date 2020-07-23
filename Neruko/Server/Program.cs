using System.Threading.Tasks;
using DryIoc;
using DryIoc.Microsoft.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Neruko.Server.Services.Discord;

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

            _discordService = await Discord.CreateService(GetConfigurationRoot());
            await CreateHostBuilder(args).Build().RunAsync();
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
