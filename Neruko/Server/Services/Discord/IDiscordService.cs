#nullable enable
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;

namespace Neruko.Server.Services.Discord
{
    public interface IDiscordService
    {
        /// <summary>
        /// Exposes the discord client
        /// </summary>
        DiscordClient Client { get; }

        ///
        /// <inheritdoc cref="DiscordClient.GetChannelAsync" />
        /// <remarks>
        /// This method returns a fall back value of null
        /// instead of throwing exception when channel is not found
        /// </remarks>
        Task<DiscordChannel?> GetChannelAsync(ulong id);
    }
}
