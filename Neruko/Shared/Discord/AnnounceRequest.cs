using System.Text.Json.Serialization;

namespace Neruko.Shared.Discord
{
    public class AnnounceRequest
    {
        /// <summary>
        /// Gets or sets the Discord channel id the message
        /// should be sent to
        /// </summary>
        [JsonPropertyName("cid")]
        public ulong ChannelId { get; set; }

        /// <summary>
        /// Gets or sets the text connect of the message to be sent
        /// </summary>
        [JsonPropertyName("msg")]
        public string? Message { get; set; }

        /// <summary>
        /// Gets or sets the announcement Purpose
        /// </summary>
        public string? Purpose { get; set; }
    }
}
