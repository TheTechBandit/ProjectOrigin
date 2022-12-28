using System.Collections.Generic;
using Newtonsoft.Json;

namespace ProjectOrigin
{
    /// <summary>Class for implementing Teams; a framework for users to form groups with each other.</summary>
    public class Team
    {
        /// <summary>The name of the team.</summary>
        public string TeamName { get; set; }
        /// <summary>A list of UUIDs for each member of the team.</summary>
        public List<ulong> MemberIDs { get; set; }
        //Members should not be stored. This causes problems with the Team Member User Objects not having the same Object ID as the UserHandler User Objects. 
        /// <summary>A list of UserAccounts of every member in the team.</summary>
        [JsonIgnore]
        public List<UserAccount> Members { get; set; }
        /// <summary>The limit for the maximum # of members allowed in the team.</summary>
        public int MemberLimit { get; set; }
        /// <summary>The team's number in combat.</summary>
        public int TeamNum { get; set; }
        /// <summary>The number of mons players in this team are allowed to bring into combat.</summary>
        public int MultiNum { get; set; }
        /// <summary>Boolean for tracking whether or not this team has been elimated from combat.</summary>
        public bool Eliminated { get; set; }
        /// <summary>The team's color's Red value.</summary>
        public int TeamR { get; set; }
        /// <summary>The team's color's Green value.</summary>
        public int TeamG { get; set; }
        /// <summary>The team's color's Blue value.</summary>
        public int TeamB { get; set; }
        /// <summary>A URL for the Team's picture.</summary>
        public string Picture { get; set; }
        /// <summary>
        /// The permission level for who is allowed to edit parts of the team.
        /// <para>OwnerOnly - only the team owner can send invites</para>
        /// <para>AllMembers - anyone can send invites</para>
        /// <para>NoPerms - anyone can send invites or kick or edit settings</para>
        /// </summary>
        public string Permissions { get; set; }
        /// <summary>If true, anyone can join the party.</summary>
        public bool OpenInvite { get; set; }

        /// <summary>Default constructor for Team.</summary>
        public Team()
        {

        }

        /// <summary>Constructor for creating a new Team.</summary>
        /// <param name="newteam">Unused boolean for differentiating from the default constructor.</param>
        public Team(bool newteam)
        {
            TeamName = NameGenerator();
            MemberIDs = new List<ulong>();
            Members = new List<UserAccount>();
            MemberLimit = -1;
            MultiNum = 1;
            Eliminated = false;
            TeamR = RandomGen.RandomInt(0, 255);
            TeamG = RandomGen.RandomInt(0, 255);
            TeamB = RandomGen.RandomInt(0, 255);
            Picture = "https://cdn.discordapp.com/emojis/732682490833141810.png?v=1";
            Permissions = "OwnerOnly";
            OpenInvite = false;
        }

        /// <summary>Add the specified User to the team.</summary>
        /// <param name="user"></param>
        public void AddMember(UserAccount user)
        {
            if (MemberLimit != -1 && Members.Count < MemberLimit)
            {
                MemberIDs.Add(user.UserId);
                Members.Add(user);
            }
            else if (MemberLimit == -1)
            {
                MemberIDs.Add(user.UserId);
                Members.Add(user);
            }
        }

        /// <summary>Checks if the Team is full or not and returns true if it is.</summary>
        /// <returns>Returns true if the Team is full.</returns>
        public bool IsTeamFull()
        {
            if (Members.Count == MemberLimit)
                return true;
            else
                return false;
        }

        /// <summary>Kick the specified user from the Team.</summary>
        /// <param name="user"></param>
        public void KickMember(UserAccount user)
        {
            if (MemberIDs.Contains(user.UserId))
            {
                MemberIDs.Remove(user.UserId);
                Members.Remove(user);
            }
        }

        /*
        FIXES THE OBJECT IDS OF THE TEAM VARIABLES
        This needs to be done because when the bot shuts down and boots back up, user objects that are saved in the team
        become desynchronized from the user objects saved in the UserHandler.
        */
        public void LoadUsers()
        {
            Members = new List<UserAccount>();

            for (int i = 0; i < MemberIDs.Count; i++)
            {
                Members.Add(UserHandler.GetUser(MemberIDs[i]));
            }
        }

        /// <summary>Gets the Team Leader and returns their user account.</summary>
        /// <returns>Returns the UserAccount of the Team Leader.</returns>
        public UserAccount TeamLeader()
        {
            if (Members.Count != 0)
                return Members[0];
            return null;
        }

        /// <summary>Checks if a specified user is the team leader.</summary>
        /// <param name="user">The user to check.</param>
        /// <returns>Returns true if the specified user is team leader, false otherwise.</returns>
        public bool IsTeamLeader(UserAccount user)
        {
            if (Members.Count != 0)
            {
                if (user.UserId == Members[0].UserId)
                    return true;
                else
                    return false;
            }

            return false;
        }

        /// <summary>Checks if a specified user has invite permissions.</summary>
        /// <param name="user">The user to check.</param>
        /// <returns>Returns true if the specified user has invite permissions, false otherwise.</returns>
        public bool CanInvite(UserAccount user)
        {
            if (IsTeamLeader(user) || Permissions.Contains("AllMembers") || Permissions.Contains("NoPerms"))
                return true;
            return false;
        }

        /// <summary>Checks if a specified user has kick permissions.</summary>
        /// <param name="user">The user to check.</param>
        /// <returns>Returns true if the specified user has kick permissions, false otherwise.</returns>
        public bool CanKick(UserAccount user)
        {
            if (IsTeamLeader(user) || Permissions.Contains("NoPerms"))
                return true;
            return false;
        }

        /// <summary>Checks if a specified user can access the settings.</summary>
        /// <param name="user">The user to check.</param>
        /// <returns>Returns true if the specified user can access settings, false otherwise.</returns>
        public bool CanAccessSettings(UserAccount user)
        {
            if (IsTeamLeader(user) || Permissions.Contains("NoPerms"))
                return true;
            return false;
        }

        /// <summary>Checks if a specified user can disband the team.</summary>
        /// <param name="user">The user to check.</param>
        /// <returns>Returns true if the specified user can disband the team, false otherwise.</returns>
        public bool CanDisband(UserAccount user)
        {
            if (IsTeamLeader(user))
                return true;
            return false;
        }

        /// <summary>Generates a new team name with a random adjective and noun from two lists of options.</summary>
        /// <returns>Returns the randomly generated name.</returns>
        public string NameGenerator()
        {
            List<string> adjs = new List<string>
            {
                "Angry", "Snoozy", "Smug", "Sad", "Poofy", "Extraordinary", "Raging", "Ecstatic", "Amazing", "Strong", "Pouty", "Muddy",
                "Drenched", "Flawless", "Tricky", "Pushy", "Greasy", "Elegant", "Scared", "Wonderful", "Hungry", "Brave", "Happy", "Clumsy",
                "Overwhelming", "Smart", "Smelly", "Handsome", "Cute", "Sleepy", "Sweet", "Slippery", "Envious", "Wiggly", "Silent", "Sneaky",
                "Spicy", "Colossal", "Weary", "Clever", "Wandering", "Dry", "Fluffy", "Midnight", "Sparkling", "Temporary", "Fearless", "League of",
                "Potent"
            };
            //Console.WriteLine($"Adjectives: {adjs.Count}");
            List<string> nouns = new List<string>
            {
                "Snorils", "Sukis", "Ooks", "Arness", "Elecutes", "Grasipups", "Meliosas", "Psygoats", "Sedimo", "Smoledge", "Stebbles",
                "Trees", "Tomatos", "Mountains", "Leaves", "River", "Lakes", "Stars", "Moons", "Wagons", "Apples", "Pineapples", "Swords",
                "Bucklers", "Bookshelves", "Lamps", "Grass", "Cactus", "Code", "Gamers", "Pirates", "Dreamers", "Society", "Fellowship",
                "Cheese", "Garbage", "Poets", "Soldiers", "Kings", "Queens", "Vagabonds", "Cookies", "Receptionists", "Secretaries",
                "Ducks", "Geese"
            };
            //Console.WriteLine($"Nouns: {nouns.Count}");

            string name = $"{adjs[RandomGen.RandomInt(0, adjs.Count - 1)]} {nouns[RandomGen.RandomInt(0, nouns.Count - 1)]}";

            return name;
        }

        /// <summary>Converts the member list into a formatted string.</summary>
        /// <returns>Returns a formatted string containing the name of every team member.</returns>
        public override string ToString()
        {
            string str = "";
            if (Members.Count == 1)
            {
                str += $"{Members[0].Name}";
            }
            else if (Members.Count == 2)
            {
                str += $"{Members[0].Name} and {Members[1].Name}";
            }
            else
            {
                for (int i = 0; i < Members.Count - 1; i++)
                {
                    str += $"{Members[i].Name}, ";
                }
                str += $"and {Members[Members.Count - 1].Name}";
            }

            return str;
        }
    }
}