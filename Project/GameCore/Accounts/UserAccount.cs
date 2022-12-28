using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace ProjectOrigin
{
    /// <summary>Class for keeping track of user-specific data.</summary>
    public class UserAccount
    {
        /// <summary>The UUID of the user.</summary>
        public ulong UserId { get; set; } = 0;
        /// <summary>The channel ID of the user's direct messages.</summary>
        public ulong DmId { get; set; } = 0;
        /// <summary>The user's mention @.</summary>
        public string Mention { get; set; } = "@";
        /// <summary>The user's name.</summary>
        public string Name { get; set; } = "";
        /// <summary>The user's character. Characters are kept separate from user data to allow potential for
        /// multiple character slots each with different data.</summary>
        public Character Char { get; set; }
        /// <summary>A URL linking to the user's avatar.</summary>
        public string AvatarUrl { get; set; } = "https://cdn.discordapp.com/attachments/453016453071896586/1057151543079608360/snoril.png";
        /// <summary>True if the user has a character made, false otherwise.</summary>
        public bool HasCharacter { get; set; }
        /// <summary>The current "prompt" or menu screen the user has from the bot. To be replaced with a more
        /// elegant solution.</summary>
        public int PromptState { get; set; }
        /// <summary>
        /// A dictionary of message IDs (ulong) that the user can respond to. The integer value represents what type of menu it is.
        /// <para>0- attack screen main</para>
        /// <para>1- move selection screen</para>
        /// <para>2- attack screen rework</para>
        /// <para>3- move selection screen rework</para>
        /// <para>4- targeting screen rework</para>
        /// </summary>
        public Dictionary<ulong, int> ReactionMessages { get; set; }
        //ulong- Message ID. ulong- User ID. Compare with ReactionMessages to decide what type of invite it is
        public Dictionary<ulong, ulong> InviteMessages { get; set; }
        /// <summary>
        /// Tracks the type of input that is expected from the user after prompting the user for input.
        /// <para>0- Invite player(s) to team</para>
        /// <para>1- Kick player(s) from team</para>
        /// <para>2- Change team name</para>
        /// <para>5- Join open team</para>
        /// <para>6- Invite player to combat lobby</para>
        /// </summary>
        /// TO BE REPLACED WITH MODALS
        public int ExpectedInput { get; set; }
        /// <summary>The channel ID that the user is expected to respond in.</summary>
        public ulong ExpectedInputLocation { get; set; }
        /// <summary>The combat lobby the player is a part of, if any.</summary>
        [JsonIgnore]        
        public CombatCreationTool CombatLobby { get; set; }

        /// <summary>Default empty constructor.</summary>
        public UserAccount()
        {

        }

        /// <summary>Constructor for a new user.</summary>
        /// <param name="newuser">Unused boolean for differentiating from the default constructor.</param>
        public UserAccount(bool newuser)
        {
            HasCharacter = false;
            PromptState = -1;
            ReactionMessages = new Dictionary<ulong, int>();
            InviteMessages = new Dictionary<ulong, ulong>();
            ExpectedInput = -1;
            ExpectedInputLocation = 0;
            CombatLobby = null;
        }

        /// <summary>Clears all reaction messages of a certain type.</summary>
        /// <param name="type">The type of reaction message to clear.</param>
        public void RemoveAllReactionMessages(int type)
        {
            if (ReactionMessages.ContainsValue(type))
            {
                foreach (var item in ReactionMessages.Where(kvp => kvp.Value == type).ToList())
                {
                    ReactionMessages.Remove(item.Key);
                }
            }
        }

        /// <summary>Returns the team the user is in, if any.</summary>
        /// <returns>Returns the team the user is in, null if not in a team.</returns>
        public Team GetTeam()
        {
            if (Char != null)
                return TownHandler.GetTown(Char.CurrentGuildId).GetTeam(this);
            return null;
        }

        /// <summary>Creates a string for debug values for the user.</summary>
        /// <returns>Returns a string of debug values.</returns>
        public string DebugString()
        {
            string str = $"UserID: {UserId}\nDmId: {DmId}\nMention: {Mention}\nName: {Name}\nAvatarUrl: {AvatarUrl}\nPromptState: {PromptState}";
            str += "\nReaction Messages- ";
            foreach (KeyValuePair<ulong, int> pair in ReactionMessages)
            {
                str += $"\nKey: {pair.Key} Value: {pair.Value}";
            }

            str += $"\nHasCharacter: {HasCharacter}";
            if (HasCharacter) str += $"\n**CHARACTER**\nName: {Char.Name}\nCurrentGuildName: {Char.CurrentGuildName}\nCurrentGuildId: {Char.CurrentGuildId}\nCombatRequest: {Char.CombatRequest}\nInCombat: {Char.InCombat}\nInPvpCombat: {Char.InPvpCombat}\nCombatId: {Char.CombatId}";
            return str;
        }

        /// <summary>Get the user's CombatLobby if they are in one, or create one if they are not in one yet.</summary>
        /// <param name="type">The type of lobby to use.</param>
        /// <param name="userid">The UserID of the user creating the lobby.</param>
        /// <returns>Returns a combat lobby.</returns>
        public CombatCreationTool GetOrCreatePvPLobby(string type)
        {
            if (HasLobby())
                return CombatLobby;
            else
            {
                CombatLobby = new CombatCreationTool(type, UserId);
                CombatLobby.AddPlayer(this);
                return CombatLobby;
            }
        }

        /// <summary>Determines whether or not the user is in a combat lobby or not.</summary>
        /// <returns>Returns true when the user is currently in a combat lobby.</returns>
        public bool HasLobby()
        {
            if (CombatLobby == null)
                return false;
            else
                return true;
        }

    }
}