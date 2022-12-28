using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace ProjectOrigin
{
    /// <summary>Static class for processing button commands. This class is controlled by the ButtonExecuted event in Connection.cs</summary>
    /// WIP: Functionality needed for only allowing the user that OWNS the menu to press the buttons in the menu.
    public static class ButtonCommands
    {
        /// <summary>Empty static constructor for ButtonCommands</summary>
        static ButtonCommands()
        {

        }

        /// <summary>Temporary method for letting the user know a button hasn't been implemented with functionality yet.</summary>
        /// <param name="cmp">The message component that was activated.</param>
        /// <param name="message">The feature that hasn't been implemented yet.</param>
        public static async Task RespondNotImplemented(SocketMessageComponent cmp, string message)
        {
            ContextIds idList = new ContextIds(cmp);

            var user = UserHandler.GetUser(idList.UserId);
            await cmp.UpdateAsync(msg => msg.Content = $"**{message} hasn't been implemented yet!**");
        }

        /// <summary>Button response for sending the user to their Party Menu.</summary>
        /// <param name="cmp">The message component that was activated.</param>
        public static async Task PartyMenu(SocketMessageComponent cmp)
        {
            ContextIds idList = new ContextIds(cmp);
            // Tests each case to make sure all circumstances for the execution of this command are valid (character exists, in correct location)
            try
            {
                UserHandler.CharacterExists(idList);
                UserHandler.ValidCharacterLocation(idList);
            }
            catch (InvalidCharacterStateException)
            {
                return;
            }

            // Get user and reset user's values for the party menu buttons.
            var user = UserHandler.GetUser(idList.UserId);
            user.Char.SwapMode = false;
            user.Char.SwapMonNum = -1;

            // Grab the url for the user's party menu and update the menu to be the Party Menu.
            string url = MessageHandler.GetImageURL(ImageGenerator.PartyMenu(user.Char.Party)).Result;
            await cmp.UpdateAsync(msg => {
                msg.Content = $"";
                msg.Embed = MonEmbedBuilder.PartyMenu(url, user);
                msg.Components = MonComponentBuilder.PartyMenu(user.Char.Party.Count);
                });
        }
    }
}