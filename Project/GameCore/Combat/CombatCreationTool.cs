using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectOrigin
{
    /// <summary>A tool for creating a new instance of combat with various settings.</summary>
    public class CombatCreationTool
    {
        //public List<List<ulong>> Players { get; set; } 
        public List<Team> Teams { get; set; }
        //Single, Double, Freeforall, 
        public string CombatType { get; set; }
        //If false, all mon are assumed to be level 50
        public bool NaturalLevels { get; set; }
        public bool ItemsOn { get; set; }
        public int MonsPerTeam { get; set; }
        public ulong PartyLeader { get; set; }

        public CombatCreationTool()
        {

        }

        public CombatCreationTool(string combatType, ulong userid)
        {
            Teams = new List<Team>();
            CombatType = combatType;
            NaturalLevels = true;
            ItemsOn = true;
            MonsPerTeam = 6;
            PartyLeader = userid;
            SetupTeams();
        }

        public void SetupTeams()
        {
            switch (CombatType)
            {
                case "single":
                    Team team1 = new Team(true);
                    team1.MemberLimit = 1;
                    team1.TeamNum = 1;

                    Team team2 = new Team(true);
                    team2.MemberLimit = 1;
                    team2.TeamNum = 2;

                    Teams.Add(team1);
                    Teams.Add(team2);
                    break;
            }
        }

        public void AddPlayer(UserAccount user)
        {
            if (!IsLobbyFull())
            {
                foreach (Team t in Teams)
                {
                    if (!t.IsTeamFull())
                    {
                        t.AddMember(user);
                        return;
                    }
                }
            }
        }

        public void RemovePlayer(UserAccount user)
        {
            for (int i = 0; i < Teams.Count; i++)
            {
                if (Teams[i].MemberIDs.Contains(user.UserId))
                {
                    Teams[i].KickMember(user);
                    user.CombatLobby = null;

                    if (user.UserId == PartyLeader)
                    {
                        foreach (Team t in Teams)
                        {
                            if (t.MemberIDs.Count > 0)
                            {
                                PartyLeader = t.MemberIDs[0];
                                return;
                            }
                        }
                    }
                    return;
                }
            }
        }

        public bool IsLobbyFull()
        {
            bool full = true;
            foreach (Team t in Teams)
            {
                if (!t.IsTeamFull())
                    full = false;
            }
            return full;
        }

        public async Task UpdateAllMenus(ulong exclude, ContextIds idList, string info)
        {
            foreach (Team t in Teams)
            {
                foreach (ulong userid in t.MemberIDs)
                {
                    if (userid != exclude)
                        await MessageHandler.UpdateMenu(UserHandler.GetUser(userid), idList.ChannelId, 12, info);
                }
            }
        }

        public async Task UpdateAllMenus(List<ulong> exclude, ContextIds idList, string info)
        {
            foreach (Team t in Teams)
            {
                foreach (ulong userid in t.MemberIDs)
                {
                    if (!exclude.Contains(userid))
                        await MessageHandler.UpdateMenu(UserHandler.GetUser(userid), idList.ChannelId, 12, info);
                }
            }
        }

        public bool IsLeader(UserAccount user)
        {
            bool leader = false;
            if (user.UserId == PartyLeader)
                leader = true;
            return leader;
        }

        public bool CheckReadyStatus()
        {
            bool ready = true;

            foreach (Team t in Teams)
            {
                foreach (ulong userid in t.MemberIDs)
                {
                    if (!UserHandler.GetUser(userid).Char.ReadyUp)
                    {
                        ready = false;
                    }
                }
            }

            return ready;
        }

        //If all users are readied up, it will kick any users who do cannot join combat (if they are already in combat) and begin combat
        public bool CheckCombatStart()
        {
            if (CheckReadyStatus())
            {
                var usercount = 0;
                foreach (Team t in Teams)
                {
                    foreach (ulong userid in t.MemberIDs)
                    {
                        usercount++;
                        var user = UserHandler.GetUser(userid);
                        if (!user.Char.HasUsableMon() || user.Char.InCombat)
                        {
                            usercount--;
                            RemovePlayer(user);
                        }
                    }
                }

                if (usercount > 1)
                    return true;
            }

            return false;
        }

        public void LevelToggle()
        {
            if (NaturalLevels)
                NaturalLevels = false;
            else
                NaturalLevels = true;
        }

        public void ItemsToggle()
        {
            if (ItemsOn)
                ItemsOn = false;
            else
                ItemsOn = true;
        }

        public void MonsToggle()
        {
            if (MonsPerTeam <= 1)
            {
                MonsPerTeam = 6;
            }
            else
            {
                MonsPerTeam--;
            }
        }

    }
}