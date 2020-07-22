using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Neruko.Server.Services;
using Neruko.Shared.Discord;

namespace Neruko.Server.Controllers
{
    /// <summary>
    /// Represents a controller responsible for making Discord announcements
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AnnounceController : ControllerBase
    {
        // Dependencies
        private readonly IDiscordService _discordService;

        public AnnounceController(IDiscordService discordService)
        {
            _discordService = discordService;
        }

        /// <summary>
        /// Announcement/Shift end point receives messages
        /// from Azure Logic app for shift management announcements
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        // POST api/Announce/Shift
        [HttpPost("Shift")]
        public async Task<IActionResult> Post(AnnounceRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.msg)) return Ok(); // Do not send empty message

            var channel = await _discordService.GetChannelAsync(request.cid);
            if (channel == null) return NoContent(); // Channel not found

            await channel.SendMessageAsync(request.msg);
            return Ok();
        }
    }
}
