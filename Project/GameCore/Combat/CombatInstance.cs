using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectOrigin
{
    /// <summary></summary>
    /// -WIP- Though functionality is relatively stable, much functionality still needs to be added and there is still a lot of testing to
    /// be done. As a result, much of this class is messy as it is constantly in need of change.
    public class CombatInstance
    {
        public ContextIds Location { get; set; }
        public List<Team> Teams { get; set; }
        public List<UserAccount> Players { get; set; }
        public Environment Environment { get; set; }
        /*
        CombatPhase determines what step of combat is currently happening
        PHASES:
        -1- Combat starting (Mons sending out)
        0- Mon(s) sent out, (activate abilities such as Intimidate)
        1- Pre turn, activate weather effects... "It is still raining!" or "The rain cleared up!"
        2- [INPUT] Awaiting both players to choose attacks 
        3- Pre-Attack phase (activate any abilities that trigger before attacks)
        4- Attacks register. Calculate whether it hit, damage, bonus effects of attacks
        5- Post-hit on MonOne/MonTwo (activate rough skin, check for death. If dead, go to phase 6)
        6- Death phase. Test if either mon died. If any did, faint them and send out the next mon if any (activate any abilities that trigger when an opponent faints such as Beast Boost)
        7- Return to phase 1
        notes: possibly remove 6, combine 3 & 4
         */
        public int CombatPhase { get; set; }
        public bool IsPvP { get; set; }
        public bool IsOneOnOne { get; set; }
        public bool CombatOver { get; set; }
        //public bool IsSoloMultiBattle { get; set; }
        //public bool IsMultiBattle { get; set; }

        public CombatInstance()
        {

        }

        public CombatInstance(ContextIds loc, UserAccount one, UserAccount two)
        {
            Location = loc;
            Teams = new List<Team>();
            Players = new List<UserAccount>();

            CreateNewTeam().AddMember(one);
            CreateNewTeam().AddMember(two);

            Environment = new Environment(true);
            IsPvP = true;
            IsOneOnOne = true;
            CombatOver = false;
            CombatPhase = -1;

            one.Char.JoinPvPCombat();
            two.Char.JoinPvPCombat();
        }

        public CombatInstance(ContextIds loc, List<Team> teams)
        {
            Location = loc;
            Teams = new List<Team>();
            Players = new List<UserAccount>();

            Environment = new Environment(true);
            IsPvP = true;
            IsOneOnOne = true;
            CombatOver = false;
            CombatPhase = -1;

            var usercount = 0;
            foreach (Team t in teams)
            {
                Teams.Add(t);
                t.TeamNum = Teams.Count;
                foreach (ulong user in t.MemberIDs)
                {
                    UserHandler.GetUser(user).Char.JoinPvPCombat();
                    usercount++;
                }
            }

            if (usercount == 2)
                IsOneOnOne = true;
            else
                IsOneOnOne = false;
        }

        public Team CreateNewTeam()
        {
            Team newteam = new Team(true);
            Teams.Add(newteam);
            newteam.TeamNum = Teams.Count;

            return newteam;
        }

        public void AddTeam(Team team)
        {
            Teams.Add(team);
            team.TeamNum = Teams.Count;
        }

        public Team GetTeam(UserAccount player)
        {
            foreach (Team t in Teams)
            {
                if (t.Members.Contains(player))
                    return t;
            }

            return null;
        }

        public Team GetTeam(BasicMon mon)
        {
            return GetTeam(GetPlayer(mon));
        }

        public UserAccount GetPlayer(BasicMon mon)
        {
            foreach (Team t in Teams)
            {
                foreach (UserAccount u in t.Members)
                {
                    if (u.Char.HasMonInParty(mon))
                        return u;
                }
            }

            return null;
        }

        public UserAccount GetOtherPlayer(UserAccount player)
        {
            if (IsOneOnOne)
            {
                if (player.UserId == Teams[0].Members[0].UserId)
                    return Teams[0].Members[0];
                else
                    return Teams[1].Members[0];
            }

            return null;
        }

        public List<BasicMon> GetAllMons()
        {
            List<BasicMon> mon = new List<BasicMon>();
            foreach (Team t in Teams)
            {
                foreach (UserAccount u in t.Members)
                {
                    foreach (BasicMon m in u.Char.ActiveMons)
                    {
                        mon.Add(m);
                    }
                }
            }

            return mon;
        }

        public List<BasicMon> GetAllEnemies(BasicMon mon)
        {
            List<BasicMon> mons = new List<BasicMon>();
            foreach (Team t in Teams)
            {
                if (!t.Members.Contains(UserHandler.GetUser(mon.OwnerID)))
                {
                    foreach (UserAccount u in t.Members)
                    {
                        foreach (BasicMon m in u.Char.ActiveMons)
                        {
                            mons.Add(m);
                        }
                    }
                }
            }

            return mons;
        }

        public void ExitCombatAll()
        {
            foreach (Team t in Teams)
            {
                foreach (UserAccount u in t.Members)
                {
                    u.Char.ExitCombat();
                }
            }
        }

        public void EndCombat()
        {
            CombatOver = true;
            ExitCombatAll();
            CombatHandler.EndCombat(this);
        }

        public async Task StartCombat()
        {
            //Make sure all users have a valid party
            foreach (Team t in Teams)
            {
                foreach (UserAccount u in t.Members)
                {
                    if (!u.Char.HasLivingParty())
                    {
                        ExitCombatAll();
                        await MessageHandler.SendMessage(Location, $"Duel canceled! {u.Mention} does not have a valid party.");
                        return;
                    }
                }
            }

            //Send out all mons. (NEEDS EDITING- COULD BE MORE THAN 2 TEAMS)
            await MessageHandler.SendMessage(Location, $"The battle between Team {Teams[0].TeamName} and Team {Teams[1].TeamName} will now begin!");

            foreach (Team t in Teams)
            {
                foreach (UserAccount u in t.Members)
                {
                    for (int i = 0; i < t.MultiNum; i++)
                    {
                        if (u.Char.HasUsableMon())
                        {
                            BasicMon sentMon = u.Char.FirstUsableMon();
                            u.Char.ActiveMons.Add(sentMon);
                            sentMon.IsCombatActive = true;
                            //sentMon.OnEnteredCombat(this);
                            await MessageHandler.SendEmbedMessage(Location, $"{u.Mention} sends out **{sentMon.Nickname}**!", MonEmbedBuilder.FieldMon(sentMon));
                        }
                    }
                }
            }

            CombatPhase = 0;
            await ResolvePhase();
        }

        public async Task CheckTeamElimination(Team team)
        {
            var teamCount = 0;
            var teamDead = 0;
            foreach (UserAccount player in team.Members)
            {
                if (!player.Char.HasLivingParty())
                    teamDead++;
                teamCount++;
            }

            if (teamCount == teamDead)
            {
                if (team.Members.Count > 1)
                    await MessageHandler.SendMessage(Location, $"Team {team.TeamName} has been eliminated!");
                team.Eliminated = true;
                await CheckBattleVictory();
            }
        }

        public async Task CheckBattleVictory()
        {
            Team team = Teams[0];
            var validTeams = 0;
            foreach (Team t in Teams)
            {
                if (!t.Eliminated)
                {
                    team = t;
                    validTeams++;
                }
            }

            if (validTeams == 1)
            {
                await MessageHandler.SendMessage(Location, $"Team {team.TeamName} is victorious!");
                EndCombat();
            }
            else if (validTeams == 0)
            {
                await MessageHandler.SendMessage(Location, $"All teams were eliminated. The battle ended in a draw.");
                EndCombat();
            }
        }

        public async Task ResolvePhase()
        {
            //PHASE 0
            if (CombatPhase == 0)
            {
                //0- check if mons need sending out, activate abilities such as Intimidate
                //Enter this phase at combat start or when a Mon faints.

                //Decides whether or not to continue combat
                foreach (Team t in Teams)
                {
                    foreach (UserAccount u in t.Members)
                    {
                        //If any mons are fainted, remove them from ActiveMons
                        for (int i = u.Char.ActiveMons.Count - 1; i >= 0; i--)
                        {
                            if (u.Char.ActiveMons[i].Fainted)
                            {
                                u.Char.ActiveMons.RemoveAt(i);
                            }
                        }
                        //If the number of ActiveMons is less than the number allowed active, send out the first usable mon if a usable mon exists until the number of ActiveMons equals the number allowed active
                        if (u.Char.ActiveMons.Count < t.MultiNum)
                        {
                            for (int i = u.Char.ActiveMons.Count; i < t.MultiNum; i++)
                            {
                                if (u.Char.HasUsableMon())
                                {
                                    BasicMon sentMon = u.Char.FirstUsableMon();
                                    u.Char.ActiveMons.Add(sentMon);
                                    sentMon.IsCombatActive = true;
                                    //sentMon.OnEnteredCombat(this);
                                    await MessageHandler.SendEmbedMessage(Location, $"{u.Mention} sends out **{sentMon.Nickname}**!", MonEmbedBuilder.FieldMon(sentMon));
                                }
                            }
                        }

                        if (u.Char.ActiveMons.Count == 0 || !u.Char.HasLivingParty())
                        {
                            u.Char.CombatEliminated = true;
                            await MessageHandler.SendMessage(Location, $"{u.Mention} has run out of mons!");
                            await CheckTeamElimination(GetTeam(u));
                            if (CombatOver)
                                return;
                        }
                    }
                }

                if (CombatOver)
                    return;
                //Continue to the next phase if combat has not ended
                else
                {
                    CombatPhase = 1;
                    await ResolvePhase();
                }
            }
            //PHASE 1
            else if (CombatPhase == 1)
            {
                //1- Pre turn, activate weather effects... "It is still raining!" or "The rain cleared up!"
                foreach (Team t in Teams)
                {
                    foreach (UserAccount u in t.Members)
                    {
                        var allbuffered = true;
                        foreach (BasicMon m in u.Char.ActiveMons)
                        {
                            if (m.BufferedMove == null)
                                allbuffered = false;
                            else
                                m.SelectedMove = m.BufferedMove;
                        }

                        if (allbuffered)
                        {
                            u.Char.CombatMovesEntered = true;
                        }
                        else
                        {
                            u.Char.CombatMovesEntered = false;
                            await MessageHandler.FightScreenNew(u.UserId);
                        }
                    }
                }

                /*Send fight screens to both players and progress to Phase 2 (wait for input)
                if(inst.PlayerOne.Char.ActiveMon.BufferedMove == null)
                    await MessageHandler.FightScreen(inst.PlayerOne.UserId);
                else
                {
                    inst.PlayerOne.Char.ActiveMon.SelectedMove = inst.PlayerOne.Char.ActiveMon.BufferedMove;
                }
                if(inst.PlayerTwo.Char.ActiveMon.BufferedMove == null)
                    await MessageHandler.FightScreen(inst.PlayerTwo.UserId);
                else
                {
                    inst.PlayerTwo.Char.ActiveMon.SelectedMove = inst.PlayerTwo.Char.ActiveMon.BufferedMove;
                }*/

                CombatPhase = 2;
            }
            //PHASE 2
            else if (CombatPhase == 2)
            {
                //2- Awaiting input from players
                var unready = 0;
                foreach (Team t in Teams)
                {
                    foreach (UserAccount u in t.Members)
                    {
                        foreach (BasicMon m in u.Char.ActiveMons)
                        {
                            if (m.SelectedMove == null)
                            {
                                unready++;
                                break;
                            }
                        }
                    }
                }

                var complete = true;
                foreach (Team t in Teams)
                {
                    foreach (UserAccount u in t.Members)
                    {
                        var finished = true;
                        foreach (BasicMon m in u.Char.ActiveMons)
                        {
                            if (m.SelectedMove == null)
                            {
                                finished = false;
                                complete = false;
                                break;
                            }
                        }
                        if (finished && !u.Char.CombatMovesEntered && unready != 0)
                        {
                            await MessageHandler.AttackEnteredTextNew(Location, u, unready);
                            u.Char.CombatMovesEntered = true;
                        }
                    }
                }

                //If all players have entered a move, apply moves
                if (complete)
                {
                    CombatPhase = 3;
                    await ResolvePhase();
                }
            }
            //PHASE 3
            else if (CombatPhase == 3)
            {
                //Pre-attack phase, activate necessary abilities
                if (!Environment.Clear)
                    await MessageHandler.SendMessage(Location, Environment.WeatherToString());

                CombatPhase = 4;
                await ResolvePhase();
            }
            //PHASE 4
            else if (CombatPhase == 4)
            {
                var mons = GetAllMons();

                //Sort all mons by their speed
                for (int j = mons.Count - 1; j > 0; j--)
                {
                    for (int i = 0; i < j; i++)
                    {
                        if (mons[i].GetAdjustedSpd() > mons[i + 1].GetAdjustedSpd())
                        {
                            var temp = mons[i];
                            mons[i] = mons[i + 1];
                            mons[i + 1] = temp;
                        }
                    }
                }

                mons.Reverse();

                //4- Attacks register. Calculate whether it hit, damage, bonus effects of attacks
                foreach (BasicMon mon in mons)
                {
                    if (!mon.Fainted)
                        await ApplyMoves(mon);
                }

                CombatPhase = 6;
                await ResolvePhase();
            }
            else if (CombatPhase == 6)
            {
                //6- Post turn phase. Reset necessary data

                //Mon post-turn ticks and resets
                foreach (BasicMon mon in GetAllMons())
                {
                    //Check if sleepy, if they are, fall asleep
                    if (mon.SleepyCheck())
                        await MessageHandler.SendMessage(Location, $"{mon.Nickname} has been afflicted with *Sleep*");
                    var damageType = mon.StatusDamage();
                    if (damageType == "Burn")
                        await MessageHandler.SendMessage(Location, $"{mon.Nickname} takes burn damage!");
                    if (mon.Status.Flinching)
                        mon.Status.Flinching = false;
                    if (mon.SelectedMove != null)
                        if (!mon.SelectedMove.Buffered)
                            mon.SelectedMove.WipeTargets();
                    mon.SelectedMove = null;
                }

                CombatPhase = 0;
                await ResolvePhase();
            }

        }

        public async Task ApplyMoves(BasicMon mon)
        {
            Console.WriteLine($"Selected move - {mon.SelectedMove.Targets.Count}");
            List<MoveResult> results = mon.SelectedMove.ApplyMove(this, mon, mon.SelectedMove.Targets);
            Console.WriteLine($"ResultsCount: {results.Count} SelectedMove: {mon.SelectedMove.Name}");
            var allmons = GetAllMons();
            foreach (BasicMon m in allmons)
            {
                mon.UpdateStats();
            }

            await MessageHandler.SendMessage(Location, $"{UserHandler.GetUser(mon.OwnerID).Char.Name}'s {mon.Nickname} uses {mon.SelectedMove.Name}!");
            bool allFail = true;
            bool allMiss = true;
            foreach (MoveResult result in results)
            {
                if (!result.Fail)
                    allFail = false;
                if (!result.Miss)
                    allMiss = false;
            }

            string addon = "";
            foreach (MoveResult result in results)
            {
                if (results.Count <= 1)
                {
                    if (result.Fail)
                        addon += $"\n{result.FailText}";
                    else if (result.Miss)
                        addon += $"\nBut it missed!";
                    else
                    {
                        foreach (string message in result.Messages)
                            addon += $"\n{message}";
                        if (result.SuperEffective)
                            addon += $"\nIt's **super effective**!";
                        if (result.NotEffective)
                            addon += $"\nIt's **not very effective**!";
                        if (result.Immune)
                            addon += $"\nIt has **no effect**!";
                        if (result.Crit)
                            addon += $"\n**Critical Hit**!";
                        foreach (string statchange in result.StatChangeMessages)
                            addon += $"\n{statchange}";
                        foreach (string status in result.StatusMessages)
                            addon += $"\n{status}";
                        if (result.Move.Type.Type.Equals("Fire") && result.Target.Status.Frozen)
                        {
                            addon += $"\n{result.Target.Nickname} was unthawed by {result.Owner.Nickname}'s fire type move!";
                            result.Target.Status.Frozen = false;
                        }
                    }
                }
                else
                {
                    if (allFail)
                    {
                        addon += $"\n{result.FailText}";
                        break;
                    }
                    else if (allMiss)
                    {
                        addon += $"\nBut it missed!";
                        break;
                    }
                    else if (result.Fail)
                        addon += $"\n{result.FailText}";
                    else if (result.Miss)
                        addon += $"\nIt missed {result.Target}!";
                    else
                    {
                        foreach (string message in result.Messages)
                            addon += $"\n{message}";
                        if (result.SuperEffective)
                            addon += $"\nIt's **super effective** against {result.Target}!";
                        if (result.NotEffective)
                            addon += $"\nIt's **not very effective** against {result.Target}!";
                        if (result.Immune)
                            addon += $"\nIt has **no effect** against {result.Target}!";
                        if (result.Crit)
                            addon += $"\n**Critical Hit** against {result.Target}!";
                        foreach (string statchange in result.StatChangeMessages)
                            addon += $"\n{statchange}";
                        foreach (string status in result.StatusMessages)
                            addon += $"\n{status}";
                        if (result.Move.Type.Type.Equals("Fire") && result.Target.Status.Frozen)
                        {
                            addon += $"\n{result.Target.Nickname} was unthawed by {result.Owner.Nickname}'s fire type move!";
                            result.Target.Status.Frozen = false;
                        }
                    }
                }
                if (result.EnemyDmg > 0 && result.Target != null)
                    await MessageHandler.UseMoveNew(Location, result.Target, addon);
                else
                    await MessageHandler.SendMessage(Location, addon);
            }

            await PostAttackPhase(mon, mon.SelectedMove.Targets, results);

            //Console.WriteLine(result1.ToString());

            //await DebugPrintMoveResult(first, second, result1, inst.Location);

            /* FOR VALUE TESTING
            string summ = "";
            summ += $"\nOwner/Mon: {first.Name}/{first.Char.ActiveMon.Nickname}";
            summ += $"\nLevel: {first.Char.ActiveMon.Level}";
            summ += $"\nPower: {first.Char.ActiveMon.SelectedMove.Power}";
            summ += $"\nAttack: {first.Char.ActiveMon.CurStats[1]}";
            (double mod, string mess) = first.Char.ActiveMon.ChangeAttStage(0);
            summ += $"\nAttack Stage Mod: {mod}";
            summ += $"\nAttack Modified: {(int)(first.Char.ActiveMon.CurStats[1]*mod)}";
            summ += $"\nDefense: {second.Char.ActiveMon.CurStats[2]}";
            (double mod2, string mess2) = second.Char.ActiveMon.ChangeDefStage(0);
            summ += $"\nDefense Stage Mod: {mod2}";
            summ += $"\nDefense Modified: {(int)(second.Char.ActiveMon.CurStats[2]*mod2)}";
            summ += $"\nModifier: {result1.Mod}";
            summ += $"\nCrit: {result1.ModCrit}";
            summ += $"\nRandom: {result1.ModRand}";
            summ += $"\nType Eff: {result1.ModType}";
            summ += $"\nDamage: {result1.EnemyDmg}";
            await MessageHandler.SendMessage(inst.Location, $"**Move Summary:**{summ}");
            //*/
        }

        //Post attack phase logic. Target is the list of mon who were hit.
        public async Task PostAttackPhase(BasicMon moveUser, List<BasicMon> targets, List<MoveResult> results)
        {
            //5- Post attack mini-phase. Check for death/on-hit abilities
            var userOwner = UserHandler.GetUser(moveUser.OwnerID);

            var swapout = false;
            BasicMon swapMon = null;
            foreach (MoveResult result in results)
            {
                if (result.Swapout != null)
                {
                    swapout = true;
                    swapMon = result.Swapout;
                }

                if (result.Hit && result.EnemyDmg > 0)
                {
                    //WHEN HIT LOGIC HERE
                }
            }

            if (moveUser.CurrentHP <= 0)
            {
                moveUser.Fainted = true;
                await MessageHandler.Faint(Location, userOwner, moveUser);
                moveUser.ExitCombat();
            }
            else if (swapout)
            {
                moveUser.ExitCombat();
                var index = userOwner.Char.ActiveMons.IndexOf(moveUser);
                userOwner.Char.ActiveMons[index] = swapMon;
                userOwner.Char.ActiveMons[index].OnEnteredCombat(this);
            }

            foreach (BasicMon t in targets)
            {
                if (t.CurrentHP <= 0)
                {
                    await FaintMon(t);
                }
            }
        }

        public async Task FaintMon(BasicMon mon)
        {
            var user = UserHandler.GetUser(mon.OwnerID);
            mon.Fainted = true;
            await MessageHandler.Faint(Location, user, mon);
            user.Char.ActiveMons.Remove(mon);
            mon.ExitCombat();
        }

    }
}