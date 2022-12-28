using Discord;
using Discord.WebSocket;

namespace ProjectOrigin
{
    /// <summary>A static class for accessing the client. To be used sparingly when certain values need to be fetched from Discord, such as user data.</summary>
    public static class ClientAccess
    {
        private static DiscordSocketClient _client;

        /// <summary>Empty static constructor.</summary>
        static ClientAccess()
        {

        }

        /// <summary>Sets the client. Executed on bot startup.</summary>
        /// <param name="client">The bot client.</param>
        public static void SetClient(DiscordSocketClient client)
        {
            _client = client;
        }

        /// <summary>Gets the user with the given user ID.</summary>
        /// <param name="id">A valid User ID.</param>
        /// <returns>Returns an IUser that is the user with the given ID.</returns>
        public static IUser GetUser(ulong id)
        {
            return _client.GetUserAsync(id).Result;
        }
    }
}
