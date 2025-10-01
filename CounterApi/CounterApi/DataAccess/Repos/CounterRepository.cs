using CounterApi.Data;
using CounterApi.Data.Model;

using Microsoft.EntityFrameworkCore;

namespace CounterApi.DataAccess.Repos
{
    public sealed class CounterRepository : ICounterRepository
    {
        private readonly CounterContext _db;
        public CounterRepository(CounterContext db) 
        { 
            _db = db;
        }

        public async Task<Counter?> GetAsync(string key, CancellationToken ct = default)
        {
          return await _db.Counters.SingleOrDefaultAsync(x => x.Page == key, ct);
        }

        public async Task<Counter> GetOrCreateAsync(string key, CancellationToken ct = default)
        {
            var existing = await _db.Counters.SingleOrDefaultAsync(x => x.Page == key, ct);
            if (existing is not null) return existing;
            var c = new Counter { Id = Guid.NewGuid(), Page = key, Count = 0, UpdateLog = new List<CounterUpdate>() };
            _db.Counters.Add(c);
            await _db.SaveChangesAsync(ct);
            return c;
        }

        public async Task<int> IncrementAsync(string key, string? message = null, CancellationToken ct = default)
        {
            using var tx = await _db.Database.BeginTransactionAsync(ct);
            var c = await _db.Counters.SingleOrDefaultAsync(x => x.Page == key, ct);
            if (c is null)
            {
                c = new Counter { Id = Guid.NewGuid(), Page = key, Count = 0, UpdateLog = new List<CounterUpdate>() };
                _db.Counters.Add(c);
                await _db.SaveChangesAsync(ct);
            }
            c.Count += 1;
            _db.Counters.Update(c);
            _db.CounterUpdates.Add(new CounterUpdate
            {
                CounterId = c.Id,
                RequestTime = DateTime.UtcNow,
                LogMessage = message ?? "inc"
            });
            await _db.SaveChangesAsync(ct);
            await tx.CommitAsync(ct);
            return c.Count;
        }

        public async Task<int?> ReadAsync(string key, CancellationToken ct = default)
        {
            var c = await _db.Counters.SingleOrDefaultAsync(x => x.Page == key, ct);
            if (c is null) return null;
            return c.Count;
        }
    }
}
