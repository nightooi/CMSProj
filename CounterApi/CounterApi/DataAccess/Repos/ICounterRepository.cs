using CounterApi.Data.Model;

namespace CounterApi.DataAccess.Repos
{
    public interface ICounterRepository
    {
        Task<Counter?> GetAsync(string key, CancellationToken ct = default);
        Task<Counter> GetOrCreateAsync(string key, CancellationToken ct = default);
        Task<int> IncrementAsync(string key, string? message = null, CancellationToken ct = default);
        Task<int?> ReadAsync(string key, CancellationToken ct = default);
    }
}
