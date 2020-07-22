#nullable enable
using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;

namespace Neruko.Server.Services
{
    /// <summary>
    /// This service will be used if the discord client did not start correctly
    /// or credentials is not given.
    /// 
    /// This prevents the app from crashing on start and allows still allows
    /// the website to operation without the Discord service.
    /// 
    /// However will throw an exception if any of the Discord service is requested
    /// </summary>
    public class NoDiscordService : IDiscordService
    {
        private readonly string _message = "Please set your `Discord:Token` secret."
                                 + Environment.NewLine
                                 + " For more info, see https://github.com/louderzone/neruko-project/issues/9 ."
                                 + Environment.NewLine + Environment.NewLine
                                 + "If you have already set the token, please make sure that token is correct and usable.";

        public NoDiscordService(string? message = null)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                _message = message;
            }
        }

        ///
        /// <inheritdoc />
        ///
        public DiscordClient Client => InvalidOperation;

        ///
        /// <inheritdoc />
        ///
        public Task<DiscordChannel?> GetChannelAsync(ulong id) => InvalidOperation;

        /// <summary>
        /// Generates the exception message
        /// </summary>
        public dynamic InvalidOperation => throw new InvalidOperationException(_message);
    }
}
