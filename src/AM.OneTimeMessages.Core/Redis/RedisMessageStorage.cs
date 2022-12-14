using StackExchange.Redis;

namespace AM.OneTimeMessages.Core.Redis
{
    public class RedisMessageStorage : IMessageStorage
    {
        private RedisConfiguration _redisConfiguration;

        private IDatabase? _db;

        private IDatabase Redis
        { 
            get 
            {
                if (_db == null)
                {
                    var multiplexer = ConnectionMultiplexer.Connect(_redisConfiguration.ConnectionString);
                    _db = multiplexer.GetDatabase();
                }

                return _db;
            }
        }

        public RedisMessageStorage(RedisConfiguration configuration)
        {
            _redisConfiguration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<string> PullAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            var value = await Redis.StringGetDeleteAsync(key);

            return value.IsNullOrEmpty ? string.Empty : value.ToString();
        }

        public async Task PushAsync(string key, string value, DateTime? ttl)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            await Redis.StringSetAsync(key, value);
        }
    }
}
