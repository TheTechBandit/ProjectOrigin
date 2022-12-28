using Discord;
using System.Threading.Tasks;

namespace ProjectOrigin
{
    /// <summary>Logger that logs messages from Discord API using Discord API's LogMessage</summary>
    public class DiscordLogger
    {
        private ILogger _logger;

        /// <summary>Constructor that takes an implemenation of the ILogger interface as a parameter</summary>
        public DiscordLogger(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>Log a LogMessage using _logger</summary>
        public Task Log(LogMessage logMsg)
        {
            _logger.Log(logMsg.Message);
            return Task.CompletedTask;
        }
    }
}