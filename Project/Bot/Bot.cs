using System.Threading.Tasks;

namespace ProjectOrigin
{
    /// <summary>Class that represents the bot and its connection.</summary>
    public class Bot
    {
        private readonly IDataStorage _storage;
        private readonly Connection _connection;

        /// <summary>Constructor to set storage and connection.</summary>
        public Bot(IDataStorage storage, Connection connection)
        {
            _storage = storage;
            _connection = connection;
        }

        /// <summary>Starts the bot by loading it with its token from the confg</summary>
        public async Task Start()
        {
            await _connection.ConnectAsync(new BotConfig
            {
                Token = _storage.RestoreObject<string>("Project\\Bot\\Config\\BotToken")
            });
        }
    }
}