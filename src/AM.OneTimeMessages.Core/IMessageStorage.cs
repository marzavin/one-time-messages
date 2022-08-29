namespace AM.OneTimeMessages.Core
{
    public interface IMessageStorage
    {
        Task PushAsync(string key, string value, DateTime? ttl);

        Task<string> PullAsync(string key);
    }
}
