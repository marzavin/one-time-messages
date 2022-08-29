namespace AM.OneTimeMessages.Core.Memory
{
    public class MemoryMessageStorage : IMessageStorage
    {
        private Dictionary<string, string> _database = new Dictionary<string, string>();

        public Task PushAsync(string key, string value, DateTime? ttl)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            _database.Add(key, value);

            return Task.CompletedTask;
        }

        public Task<string> PullAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (_database.ContainsKey(key))
            {
                var value = _database[key];
                _database.Remove(key);

                return Task.FromResult(value);
            } 
            
            return Task.FromResult(string.Empty);
        }
    }
}