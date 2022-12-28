using System.Collections.Generic;

namespace ProjectOrigin
{
    /// <summary>An object that stores all information related to "Towns" (aka Guilds/Servers)</summary>
    public class TownAccount
    {
        /// <summary>The ID of the Guild this town is associated with.</summary>
        public ulong GuildId { get; set; }
        /// <summary>The name of the town.</summary>
        public string Name { get; set; }
        /// <summary>The list of Teams that exist within the town.</summary>
        public List<Team> Teams { get; set; }

        /// <summary>Default empty constructor.</summary>
        public TownAccount()
        {

        }

        /// <summary>A constructor for creating new towns.</summary>
        /// <param name="newtown">Unused boolean for differentiating from the default constructor.</param>
        public TownAccount(bool newtown)
        {
            Teams = new List<Team>();
        }

        /// <summary>Reloads all users into the team list- this is done once after startup since they are not stored.</summary>
        public void UpdateTeams()
        {
            foreach (Team t in Teams)
            {
                t.LoadUsers();
            }
        }

        /// <summary>Gets the team of a specified user.</summary>
        /// <param name="user">The UserAccount to find the team of.</param>
        /// <returns>Returns the Team specified user is a part of, if any.</returns>
        public Team GetTeam(UserAccount user)
        {
            foreach (Team t in Teams)
            {
                foreach (ulong u in t.MemberIDs)
                {
                    if (u == user.UserId)
                        return t;
                }
            }

            return null;
        }

    }
}