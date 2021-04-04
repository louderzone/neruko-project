using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Exceptions;

namespace Neruko.Server.Services.Discord
{
    ///
    /// <inheritdoc />
    ///
    public class DiscordService : IDiscordService
    {
        public DiscordService(DiscordClient client)
        {
            Client = client;
        }

        ///
        /// <inheritdoc />
        ///
        public async Task<DiscordChannel?> GetChannelAsync(ulong id)
        {
            try
            {
                return await Client.GetChannelAsync(id);
            }
            catch (NotFoundException)
            {
                return default;
            }
        }

        ///
        /// <inheritdoc />
        ///
        public DiscordClient Client { get; }
    }
}
