using Discord.Commands;
using Discord.WebSocket;

namespace ProjectOrigin
{
    /// <summary>Class for storing the UUIDs of various important context items such as user and channel ids.</summary>
    public class ContextIds
    {
        public ulong UserId { get; set; }
        public ulong ChannelId { get; set; }
        public ulong GuildId { get; set; }
        public ulong MessageId { get; set; }

        public ContextIds()
        {

        }

        /// <summary>Main constructor that takes the command context and extracts IDs.</summary>
        /// <param name="context">The command context.</param>
        public ContextIds(SocketCommandContext context)
        {
            UserId = context.User.Id;
            ChannelId = context.Channel.Id;
            GuildId = context.Guild.Id;
            MessageId = context.Message.Id;
        }

        /// <summary>Main constructor that takes the command context and extracts IDs.</summary>
        /// <param name="context">The command context.</param>
        public ContextIds(SocketSlashCommand context)
        {
            UserId = context.User.Id;
            ChannelId = context.Channel.Id;
            GuildId = (ulong)context.GuildId;
            MessageId = context.Id;
        }

        /// <summary>Main constructor that takes the command context and extracts IDs.</summary>
        /// <param name="context">The command context.</param>
        public ContextIds(SocketMessageComponent context)
        {
            UserId = context.User.Id;
            ChannelId = context.Channel.Id;
            GuildId = (ulong)context.GuildId;
            MessageId = context.Id;
        }
    }
}