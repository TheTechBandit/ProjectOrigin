using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using Discord;

namespace ProjectOrigin
{
    /// <summary>Main class for running the bot.</summary>
    internal class Program
    {
        /// <summary>Main method for starting the program.</summary>
        private static async Task Main()
        {
            DiscordSocketClient client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose
            });

            CommandService commands = new CommandService(new CommandServiceConfig
            {
                DefaultRunMode = RunMode.Async,
                CaseSensitiveCommands = false,
                LogLevel = LogSeverity.Verbose
            });

            var bot = new Bot(new JsonStorage(), new Connection(new DiscordLogger(new Logger()), client, commands));
            await bot.Start();
        }
    }


}