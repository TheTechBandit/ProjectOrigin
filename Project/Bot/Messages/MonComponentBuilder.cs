using Discord;

namespace ProjectOrigin
{
    /// <summary>Class for building sets of Components such as buttons and select menus.</summary>
    public static class MonComponentBuilder
    {

        /// <summary>Empty static constructor</summary>
        static MonComponentBuilder()
        {

        }

        /// <summary>Main menu component builder</summary>
        public static MessageComponent MainMenu()
        {
            var builder = new ComponentBuilder();

            builder.AddRow(new ActionRowBuilder()
                .WithButton(" ", "MainMenuLocation", ButtonStyle.Success, Emote.Parse("<:lo:741373459736821853>"))
                .WithButton(" ", "MainMenuParty", ButtonStyle.Success, Emote.Parse("<:sn:741373460177223680>"))
                .WithButton(" ", "MainMenuBag", ButtonStyle.Success, Emote.Parse("<:ba:741373460009451640>")));
            builder.AddRow(new ActionRowBuilder()
                .WithButton(" ", "MainMenuDex", ButtonStyle.Success, Emote.Parse("<:de:741373459703267340>"))
                .WithButton(" ", "MainMenuTeam", ButtonStyle.Success, Emote.Parse("<:te:741373460227555438>"))
                .WithButton(" ", "MainMenuPvP", ButtonStyle.Success, Emote.Parse("<:pv:741373459791347767>"))
                .WithButton(" ", "MainMenuSettings", ButtonStyle.Secondary, Emote.Parse("<:se:741373460198195220>")));

            return builder.Build();
        }

        /// <summary>Party menu component builder</summary>
        /// <param name="partyCount">The amount of mons in the users party for adding the correct amount of swap buttons.</param>
        public static MessageComponent PartyMenu(int partyCount)
        {
            var builder = new ComponentBuilder();

            builder.AddRow(new ActionRowBuilder()
                .WithButton(" ", "BackMainMenu", ButtonStyle.Success, Emote.Parse("<:back1:735583967046271016>"))
                .WithButton(" ", "PartyMenuSwap", ButtonStyle.Success, Emote.Parse("<:swap:736070692373659730>")));

            // Add number buttons based on the number of party members so each party member can be selected.
            ActionRowBuilder numberRow = new ActionRowBuilder();
            for (int i = 1; i <= partyCount; i++)
            {
                numberRow.WithButton(" ", $"PartyNumber{i}", ButtonStyle.Secondary, new Emoji($"{i}\u20E3"));
            }
            builder.AddRow(numberRow);

            return builder.Build();
        }
    }
}
