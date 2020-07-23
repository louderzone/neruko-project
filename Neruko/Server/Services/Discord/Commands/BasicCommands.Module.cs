using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace Neruko.Server.Services.Discord.Commands
{
    /// <summary>
    /// Represents basic bot commands such as greetings
    /// </summary>
    public class BasicCommands : ICommandModule
    {
        /// <summary>
        /// The `/neruko {msg}` command wil allow users to speak as the bot
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [Command("neruko")]
        [Description("Speak as Neruko :)")]
        public async Task Reply(CommandContext ctx,
            [Description("Content of the message")] string message)
        {
            await ctx.Message.DeleteAsync("User is speaking as bot");
            await ctx.Channel.SendMessageAsync(message);
        }
    }
}
