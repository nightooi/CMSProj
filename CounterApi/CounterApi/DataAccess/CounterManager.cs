using CounterApi.DataAccess.Repos;

namespace CounterApi.DataAccess
{
    public sealed class CounterManager : ICounterManager
    {
        private readonly ICounterRepository _repo;
        public CounterManager(ICounterRepository repo)
        { 
            _repo = repo;
        }

        public async Task<int> IncrementAsync(string key, CancellationToken ct = default) 
        { 
           return await _repo.IncrementAsync(Normalize(key), null, ct);
        }

        public async Task<int?> ReadAsync(string key, CancellationToken ct = default)
        { 
           return await _repo.ReadAsync(Normalize(key), ct); 
        }

        private static string Normalize(string k) { 
            
            return string.IsNullOrWhiteSpace(k) ? "home" : k.Trim().ToLowerInvariant(); 
        }
    }
}
