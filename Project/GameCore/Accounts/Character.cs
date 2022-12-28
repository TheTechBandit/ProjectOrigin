using System.Collections.Generic;

namespace ProjectOrigin
{
    /// <summary>Class for tracking all game data for a user.</summary>
    public class Character
    {
        /// <summary>The character's name.</summary>
        public string Name { get; set; }
        /// <summary>The Mention string for the user who owns this character.</summary>
        public string Mention { get; set; }
        /// <summary>The Guild ID for the Guild this character is currently in.</summary>
        public ulong CurrentGuildId { get; set; }
        /// <summary>The name of the guild for the Guild this character is currently in.</summary>
        public string CurrentGuildName { get; set; }
        /// <summary>The character's selected mon.</summary>
        public BasicMon ActiveMon { get; set; }
        /// <summary>List of mons that are actively participating in combat.</summary>
        public List<BasicMon> ActiveMons { get; set; }
        /// <summary>List of mons that are in the party.</summary>
        public List<BasicMon> Party { get; set; }
        /// <summary>The list of mons that are in storage.</summary>
        public List<BasicMon> PC { get; set; }
        /// <summary>A boolean to track when the user has selected "ready up" in a combat lobby.</summary>
        public bool ReadyUp { get; set; }
        /// <summary>The user ID of a user that is requesting a duel.</summary>
        public ulong CombatRequest { get; set; }
        /// <summary>True if the character is in any combat.</summary>
        public bool InCombat { get; set; }
        /// <summary>True if the character is in pvp combat.</summary>
        public bool InPvpCombat { get; set; }
        /// <summary>True if the character has been elimated from combat.</summary>
        public bool CombatEliminated { get; set; }
        /// <summary>The ID of the combat instance this user is currently in, if any.</summary>
        public int CombatId { get; set; }
        /// <summary>A variable to track which mon to use for move screen selection.</summary>
        public int MoveScreenNum { get; set; }
        /// <summary>The currently selected page for Targeting (for use in multi-battles with many targeting choices)</summary>
        public int TargetPage { get; set; }
        /// <summary>True if the user has selected all of their combat moves.</summary>
        public bool CombatMovesEntered { get; set; }
        /// <summary>Holds the value of the mon currently selected to be swapped with another.</summary>
        public int SwapMonNum { get; set; }
        /// <summary>True if the user has activated swap mode on the party menu.</summary>
        public bool SwapMode { get; set; }

        /// <summary>Empty constructor for deserialization.</summary>
        public Character()
        {

        }

        /// <summary>Constructor for building a new character.</summary>
        /// <param name="newchar">Unused boolean for differentiating from the default constructor.</param>
        public Character(bool newchar)
        {
            ActiveMons = new List<BasicMon>();
            Party = new List<BasicMon>();
            PC = new List<BasicMon>();
            ReadyUp = false;
            InCombat = false;
            InPvpCombat = false;
            CombatEliminated = false;
            CombatId = -1;
            MoveScreenNum = 0;
            TargetPage = 0;
            CombatMovesEntered = false;
            SwapMonNum = -1;
            SwapMode = false;
        }

        /// <summary>Exits the user from combat.</summary>
        public void ExitCombat()
        {
            InCombat = false;
            InPvpCombat = false;
            CombatEliminated = false;
            CombatRequest = 0;
            CombatId = -1;
            MoveScreenNum = 0;
            TargetPage = 0;
            CombatMovesEntered = false;
            foreach (BasicMon mon in Party)
            {
                mon.ExitCombat();
            }
            ActiveMon = null;
            ActiveMons.Clear();
        }

        /// <summary>Gets the first combat-ready mon in the party.</summary>
        /// <returns>Returns the first combat-ready mon in the character's party.</returns>
        public BasicMon FirstUsableMon()
        {
            for (int i = 0; i < Party.Count; i++)
            {
                if (Party[i].CurrentHP > 0 && !Party[i].IsCombatActive)
                {
                    return Party[i];
                }
            }
            return null;
        }


        /// <summary>Gets the first combat-ready mon in the party with a list of mons to exclude.</summary>
        /// <param name="exclude">A list of mons to exclude from the search.</param>
        /// <returns>Returns the first combat-ready mon in the character's party.</returns>
        public BasicMon FirstUsableMon(List<BasicMon> exclude)
        {
            for (int i = 0; i < Party.Count; i++)
            {
                if (Party[i].CurrentHP > 0 && !Party[i].IsCombatActive)
                {
                    foreach (BasicMon exclusion in exclude)
                        if (Party[i] != exclusion)
                            return Party[i];
                }
            }
            return null;
        }

        /// <summary>Checks if the character has any combat-ready mons left.</summary>
        /// <returns>Returns true if the character has combat-ready mons in the party, false if otherwise.</returns>
        public bool HasUsableMon()
        {
            for (int i = 0; i < Party.Count; i++)
            {
                if (Party[i].CurrentHP > 0 && !Party[i].IsCombatActive)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>Checks if the character has a specified mon in their party.</summary>
        /// <param name="mon">The mon to check for.</param>
        /// <returns>Returns true if the character has the specified mon in their party.</returns>
        public bool HasMonInParty(BasicMon mon)
        {
            foreach (BasicMon m in Party)
            {
                if (mon == m)
                    return true;
            }
            return false;
        }

        /// <summary>Checks if the user's party has any living mons.</summary>
        /// <returns>Returns false if every mon in the party is fainted, there are no mons in the party, or there are too many mons in the party.</returns>
        public bool HasLivingParty()
        {
            var dead = 0;
            foreach (BasicMon mon in Party)
            {
                if (mon.Fainted)
                {
                    dead++;
                }
            }
            if (dead == Party.Count || Party.Count < 1 || Party.Count > 6)
            {
                return false;
            }

            return true;
        }

        /// <summary>Gets the number of living mons in the party.</summary>
        /// <returns>Returns the number of living mons in the party.</returns>
        public int LivingPartyNum()
        {
            var living = 0;
            foreach (BasicMon mon in Party)
            {
                if (!mon.Fainted)
                {
                    living++;
                }
            }

            return living;
        }

        /// <summary>Checks if the party is full and returns true if it is.</summary>
        /// <returns>Returns true if the party is full.</returns>
        public bool IsPartyFull()
        {
            if (Party.Count >= 6)
                return true;
            else
                return false;
        }

        /// <summary>Sets the character's values as being in PvP combat.</summary>
        public void JoinPvPCombat()
        {
            InCombat = true;
            InPvpCombat = true;
            CombatRequest = 0;
            CombatId = CombatHandler.NumberOfInstances();
        }

        /// <summary>Toggle the ReadyUp boolean.</summary>
        public void ReadyToggle()
        {
            ReadyUp = !ReadyUp;
        }
    }
}