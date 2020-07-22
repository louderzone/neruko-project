using System;
using System.Collections.Generic;
using System.Text;

namespace Neruko.Shared.Discord
{
    public class AnnounceRequest
    {
        /// <summary>
        /// Gets or sets the Discord channel id the message
        /// should be sent to
        /// </summary>
        public ulong cid { get; set; }

        /// <summary>
        /// Gets or sets the text connect of the message to be sent
        /// </summary>
        public string msg { get; set; } = "";

        /// <summary>
        /// Gets or sets the announcement purpose
        /// </summary>
        public string? purpose { get; set; }
    }
}
