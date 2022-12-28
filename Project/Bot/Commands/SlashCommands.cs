using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace ProjectOrigin
{
    /// <summary>Static class for processing slash commands. This class is controlled by the SlashCommandExecuted event in Connection.cs</summary>
    public static class SlashCommands
    {
        /// <summary>Empty static constructor for SlashCommands</summary>
        static SlashCommands()
        {

        }

        /// <summary>Creates and sends the Main Menu for the invoking user.</summary>
        /// <param name="cmd">The slash command that was sent.</param>
        public static async Task MainMenu(SocketSlashCommand cmd)
        {
            ContextIds idList = new ContextIds(cmd);

            // Tests each case to make sure all circumstances for the execution of this command are valid (character exists, in correct location)
            try
            {
                UserHandler.CharacterExists(idList);
                UserHandler.ValidCharacterLocation(idList);
            }
            catch (InvalidCharacterStateException e)
            {
                // Respond with an error message
                await cmd.RespondAsync($"{e.Message}", null, false, false);
                return;
            }

            // Get user and respond with main menu
            var user = UserHandler.GetUser(idList.UserId);
            await cmd.RespondAsync($"", embed: MonEmbedBuilder.MainMenu(user), components: MonComponentBuilder.MainMenu());
        }
    }
}
