using System;
using Discord;

namespace ProjectOrigin
{
    /// <summary>Builds embeds through Discord's EmbedBuilder. Used primarily for user interfaces.</summary>
    /// -OUTDATED-
    /// This class is outdated. With the addition of Components in the V3 Discord.Net update, most user interface menus needs a redesign
    /// to accommodate for the new user input methods such as Buttons and Modals. Much of this code is messy, outdated, uncommented, and 
    /// does not represent the direction of this project. It will be updated once more functionality for Buttons and Modals has been implemented.
    public static class MonEmbedBuilder
    {
        /// <summary>Empty static constructor</summary>
        static MonEmbedBuilder()
        {

        }

        public static Embed MonStats(BasicMon mon)
        {
            var moves = "";
            foreach (BasicMove move in mon.ActiveMoves)
            {
                if (move.Name != "None")
                {
                    moves += $"**{move.Name}**, ";
                }
            }
            moves = moves.Substring(0, moves.Length - 2);

            var user = UserHandler.GetUser(mon.OwnerID);

            var builder = new EmbedBuilder()
            .WithTitle(mon.TypingToString())
            .WithDescription($"DEX NO. {mon.DexNum}")
            .WithColor(139, 87, 42)
            .WithTimestamp(DateTimeOffset.FromUnixTimeMilliseconds(1560726232528))
            .WithFooter(footer => {
                footer
                    .WithText($"Caught By {UserHandler.GetUser(mon.CatcherID).Name}");
            })
            .WithImageUrl(mon.ArtURL)
            .WithAuthor(author => {
                author
                    .WithName(mon.Species)
                    .WithIconUrl(mon.ArtURL);
            })
            .AddField("Level", mon.Level, true)
            .AddField("HP", $"{mon.CurrentHP}/{mon.TotalHP}", true)
            .AddField("Stats", mon.CurStatsToString())
            .AddField("Moves", moves)
            .AddField("Nature", mon.Nature, true)
            .AddField("Ability", "**NOT IMPLEMENTED YET**", true)
            .AddField("Held Item", "**NOT IMPLEMENTED YET**", true)
            .AddField("Gender", mon.Gender, true)
            .AddField("IVs", mon.IvsToString())
            .AddField("EVs", mon.EvsToString());
            var embed = builder.Build();

            return embed;
        }

        public static Embed MonDex(BasicMon mon)
        {
            var builder = new EmbedBuilder()
            .WithTitle(mon.TypingToString())
            .WithColor(139, 87, 42)
            .WithFooter(footer => {
                footer
                    .WithText($"Dex No. {mon.DexNum}");
            })
            .WithImageUrl(mon.ArtURL)
            .WithAuthor(author => {
                author
                    .WithName(mon.Species)
                    .WithIconUrl(mon.ArtURL);
            })
            .AddField("Stats", mon.BaseStatsToString())
            .AddField("Dex Entry", mon.DexEntry);
            var embed = builder.Build();

            return embed;
        }

        public static Embed MonSendOut(UserAccount user, BasicMon mon)
        {
            int r = mon.HPGradient()[0];
            int g = mon.HPGradient()[1];
            int b = mon.HPGradient()[2];

            var builder = new EmbedBuilder()
            .WithTitle($"{user.Name} sends out **{mon.Nickname}**!")
            .WithThumbnailUrl(mon.ArtURL)
            .WithFooter(user.Name, user.AvatarUrl)
            .WithColor(r, g, b);
            var embed = builder.Build();

            return embed;
        }

        public static Embed FieldMon(BasicMon mon)
        {
            int r = mon.HPGradient()[0];
            int g = mon.HPGradient()[1];
            int b = mon.HPGradient()[2];

            string statuses = "";
            if (mon.Status.Paraylzed)
            {
                statuses += "<:Paralyzed:716427812558602250>";
            }
            if (mon.Status.Burned)
            {
                statuses += "<:Burn:717232618327769141>";
            }

            var user = UserHandler.GetUser(mon.OwnerID);

            var builder = new EmbedBuilder()
            .WithFooter(user.Name, user.AvatarUrl)
            .WithTitle($"Lv. {mon.Level}")
            .WithThumbnailUrl(mon.ArtURL)
            .WithColor(r, g, b)
            .WithAuthor($"{UserHandler.GetUser(mon.OwnerID).Char.Name}'s {mon.Nickname} {mon.GenderSymbol}")
            .WithDescription($"{mon.CurrentHP}/{mon.TotalHP} HP {statuses}");
            var embed = builder.Build();

            return embed;
        }

        public static Embed EmptyPartySpot(int num)
        {
            string spots = "";
            for (int i = 0; i < num; i++)
            {
                spots += "[ Empty ]\n\n";
            }

            var builder = new EmbedBuilder()
            .WithTitle(spots)
            .WithColor(62, 255, 62);
            var embed = builder.Build();

            return embed;
        }

        public static Embed FightScreen(BasicMon mon)
        {
            var user = UserHandler.GetUser(mon.OwnerID);

            var builder = new EmbedBuilder()
            .WithFooter(user.Name, user.AvatarUrl)
            .WithTitle(mon.Nickname)
            .WithColor(62, 255, 62)
            .WithThumbnailUrl(mon.ArtURL)
            .WithImageUrl("https://cdn.discordapp.com/attachments/452818546175770624/626432333414924288/fight_screen.png");
            var embed = builder.Build();

            return embed;
        }

        public static Embed MoveScreen(BasicMon mon)
        {
            var user = UserHandler.GetUser(mon.OwnerID);

            var builder = new EmbedBuilder()
            .WithFooter(user.Name, user.AvatarUrl)
            .WithTitle("**Moves**")
            .WithColor(62, 255, 62)
            .AddField(mon.ActiveMoves[0].Name, mon.ActiveMoves[0].Description + $"\nPP: {mon.ActiveMoves[0].CurrentPP}/{mon.ActiveMoves[0].MaxPP}", true)
            .AddField(mon.ActiveMoves[1].Name, mon.ActiveMoves[1].Description + $"\nPP: {mon.ActiveMoves[1].CurrentPP}/{mon.ActiveMoves[1].MaxPP}", true)
            .AddField(mon.ActiveMoves[2].Name, mon.ActiveMoves[2].Description + $"\nPP: {mon.ActiveMoves[2].CurrentPP}/{mon.ActiveMoves[2].MaxPP}", true)
            .AddField(mon.ActiveMoves[3].Name, mon.ActiveMoves[3].Description + $"\nPP: {mon.ActiveMoves[3].CurrentPP}/{mon.ActiveMoves[3].MaxPP}", true);
            var embed = builder.Build();

            return embed;
        }

        public static Embed MoveScreenNew(BasicMon mon)
        {
            var user = UserHandler.GetUser(mon.OwnerID);

            var builder = new EmbedBuilder()
            .WithFooter(user.Name, user.AvatarUrl)
            .WithTitle("**Moves**")
            .WithColor(62, 255, 62)
            .WithThumbnailUrl(mon.ArtURL)
            .AddField("1\u20E3 " + mon.ActiveMoves[0].Name, mon.ActiveMoves[0].Description + $"\nPP: {mon.ActiveMoves[0].CurrentPP}/{mon.ActiveMoves[0].MaxPP}", false)
            .AddField("2\u20E3 " + mon.ActiveMoves[1].Name, mon.ActiveMoves[1].Description + $"\nPP: {mon.ActiveMoves[1].CurrentPP}/{mon.ActiveMoves[1].MaxPP}", false)
            .AddField("3\u20E3 " + mon.ActiveMoves[2].Name, mon.ActiveMoves[2].Description + $"\nPP: {mon.ActiveMoves[2].CurrentPP}/{mon.ActiveMoves[2].MaxPP}", false)
            .AddField("4\u20E3 " + mon.ActiveMoves[3].Name, mon.ActiveMoves[3].Description + $"\nPP: {mon.ActiveMoves[3].CurrentPP}/{mon.ActiveMoves[3].MaxPP}", false);
            var embed = builder.Build();

            return embed;
        }

        public static Embed TargetingScreen(UserAccount user, BasicMon mon)
        {
            var builder = new EmbedBuilder()
            .WithTitle("**Choose A Target**")
            .WithColor(62, 255, 62)
            .WithFooter($"Pg. {user.Char.TargetPage + 1}/{Math.Ceiling(mon.SelectedMove.ValidTargets.Count / 9.0)}");
            for (int i = user.Char.TargetPage * 9; i < mon.SelectedMove.ValidTargets.Count; i++)
            {
                if (i > (user.Char.TargetPage * 9) + 8)
                    break;
                var target = mon.SelectedMove.ValidTargets[i];
                builder.AddField($"{i + 1}\u20E3 - {UserHandler.GetUser(target.OwnerID).Char.Name}'s {target.Nickname}", false);
            }
            var embed = builder.Build();

            return embed;
        }

        public static Embed MainMenu(UserAccount user)
        {
            var builder = new EmbedBuilder()
            .WithTitle($"__{user.Char.Name}__\n<:lo:741373459736821853> Locations\n<:sn:741373460177223680> Party\n<:ba:741373460009451640> Bag\n<:de:741373459703267340> Dex\n<:te:741373460227555438> Team\n<:pv:741373459791347767> PvP\n<:se:741373460198195220> Settings")
            //Too many characters 
            //.WithTitle($"__Ghub__\n<:LocationEmote:732673934184677557> Locations\n<:snoril:580944131535273991> Party\n<:Bag:732676561341251644> Bag\n<:Dex:732679405704445956> Dex\n<:Team:732682490833141810> Team\n<:PvP:732680927242878979> PvP\n<:Settings:732683469485899826> Settings")
            //Alternate solution
            //.AddField($"__Ghub__", $"<:LocationEmote:732673934184677557> **Locations**\n<:snoril:580944131535273991> **Party**\n<:Bag:732676561341251644> **Bag**\n<:Dex:732679405704445956> **Dex**\n<:Team:732682490833141810> **Team**\n<:PvP:732680927242878979> **PvP**\n<:Settings:732683469485899826> **Settings**", false)
            .WithThumbnailUrl($"{user.AvatarUrl}")
            .WithColor(62, 255, 62);

            var embed = builder.Build();

            return embed;
        }

        public static Embed ModifyAsyncTestPageOne()
        {
            var builder = new EmbedBuilder()
            .WithTitle("**Top 3 Anime Deaths**")
            .WithColor(62, 255, 62)
            .WithFooter($"Page 1/2")
            .AddField("1. Spongebob", false)
            .AddField("2. Hank Hill", false)
            .AddField("3. Ghub", false);

            var embed = builder.Build();

            return embed;
        }

        public static Embed ModifyAsyncTestPageTwo()
        {
            var builder = new EmbedBuilder()
            .WithTitle("**Top 3 Anime Betrayals**")
            .WithColor(62, 255, 62)
            .WithFooter($"Page 2/2")
            .AddField("1. Patrick kills Spongebob", false)
            .AddField("2. Bobby Hill kills Hank Hill", false)
            .AddField("3. Zsaur kills Ghub", false);

            var embed = builder.Build();

            return embed;
        }

        public static Embed PartyMenu(string image, UserAccount user)
        {
            var builder = new EmbedBuilder()
            .WithFooter(user.Name, user.AvatarUrl)
            .WithTitle("Party")
            .WithColor(62, 255, 62)
            .WithImageUrl(image);

            var embed = builder.Build();

            return embed;
        }

        public static Embed TeamMenu(UserAccount user)
        {
            Team t = user.GetTeam();
            var builder = new EmbedBuilder();

            if (t != null)
            {
                string members = "";
                foreach (UserAccount u in t.Members)
                {
                    members += $"{u.Char.Name}\n";
                }

                builder.WithTitle($"**{t.TeamName}**")
                .AddField($"**Members**", $"{members}", false)
                .WithFooter(user.Name, user.AvatarUrl)
                .WithColor(t.TeamR, t.TeamG, t.TeamB);
                if (t.Picture != "")
                    builder.WithThumbnailUrl(t.Picture);
            }
            else
            {
                builder.WithTitle($"**Team**")
                .AddField($"You Do Not Have A Team", $"To create a team, press the Team button or use the \"createteam\" command.", false)
                .WithFooter(user.Name, user.AvatarUrl)
                .WithColor(62, 255, 62);
            }

            var embed = builder.Build();
            return embed;
        }

        public static Embed TeamInviteMenu(UserAccount user, UserAccount invitedUser)
        {
            var t = user.GetTeam();
            var builder = new EmbedBuilder()
            .WithTitle("Team Invite")
            .AddField($"{invitedUser.Name}, you have been invited to {user.Name}'s Team", "Click the checkmark to join, or the X to decline.", false)
            .WithFooter(invitedUser.Name, invitedUser.AvatarUrl)
            .WithColor(t.TeamR, t.TeamG, t.TeamB);
            if (t.Picture != "")
                builder.WithThumbnailUrl(t.Picture);

            var embed = builder.Build();
            return embed;
        }

        public static Embed LobbyInviteMenu(UserAccount user, UserAccount invitedUser)
        {
            var lobby = user.CombatLobby;
            var builder = new EmbedBuilder()
            .WithTitle("Team Invite")
            .AddField($"{invitedUser.Name}, you have been invited to {user.Name}'s Combat Lobby", "Click the checkmark to join, or the X to decline.", false)
            .WithFooter(invitedUser.Name, invitedUser.AvatarUrl)
            .WithColor(255, 0, 0);
            if (lobby.CombatType == "single")
                builder.WithThumbnailUrl("https://cdn.discordapp.com/attachments/453016453071896586/809578784226541583/SingleBattle.png");
            if (lobby.CombatType == "double")
                builder.WithThumbnailUrl("https://cdn.discordapp.com/attachments/453016453071896586/809578917467127869/DoubleBattle.png");
            if (lobby.CombatType == "ffa")
                builder.WithThumbnailUrl("https://cdn.discordapp.com/attachments/453016453071896586/809578923478351902/FFA.png");
            if (lobby.CombatType == "custom")
                builder.WithThumbnailUrl("https://cdn.discordapp.com/attachments/453016453071896586/809578936077254686/custombattle.png");

            var embed = builder.Build();
            return embed;
        }

        public static Embed TeamSettingsMenu(UserAccount user)
        {
            var t = user.GetTeam();
            string openclosed = "";
            if (t.OpenInvite)
                openclosed = "<:unlocked:736491703091069031> [Open] - Toggle team to be CLOSED";
            else
                openclosed = "<:locked:736491688712732733> [Closed] - Toggle team to be OPEN";

            //WIP DO THIS NEXT
            var builder = new EmbedBuilder()
            //.WithTitle($"__{user.GetTeam().TeamName} Settings__")
            .AddField($"__{user.GetTeam().TeamName} Settings__", $"<:edit:736488507895447622> - Edit Name\n<:addpicturegreen:736489932297863248> - Edit Image\n<:rgb:736489012595785818> [{t.TeamR}, {t.TeamG}, {t.TeamB}] - Edit RGB\n<:permissions:736476627675906078> [{t.Permissions}] - Edit Permissions\n{openclosed}", false)
            .WithFooter(user.Name, user.AvatarUrl)
            .WithColor(t.TeamR, t.TeamG, t.TeamB);
            if (t.Picture != "")
                builder.WithThumbnailUrl(t.Picture);

            var embed = builder.Build();
            return embed;
        }

        public static Embed PvPMainMenu(UserAccount user)
        {
            var builder = new EmbedBuilder();
            builder.WithTitle("Create A Lobby")
            .WithFooter(user.Name, user.AvatarUrl)
            .WithColor(62, 255, 62)
            .WithImageUrl("https://cdn.discordapp.com/attachments/353373925524373516/752342885185224714/Combat_Menu.png");

            var embed = builder.Build();
            return embed;
        }

        public static Embed PvPLobby(CombatCreationTool lobb, String url, UserAccount user)
        {
            var builder = new EmbedBuilder();
            builder.WithColor(255, 0, 0)
            .WithFooter(user.Name, user.AvatarUrl)
            .WithImageUrl(url);

            var embed = builder.Build();
            return embed;
        }

    }
}