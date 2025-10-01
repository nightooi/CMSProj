namespace CounterApi.DataAccess
{
    public interface ICounterManager
    {
        Task<int> IncrementAsync(string key, CancellationToken ct = default);
        Task<int?> ReadAsync(string key, CancellationToken ct = default);
    }
}
