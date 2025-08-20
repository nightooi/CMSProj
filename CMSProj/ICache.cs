public interface ICache<Tkey, TValue>
{
    public IReadOnlyDictionary<Tkey, TValue> Cache { get; }
    public Task<TValue> FindAsync(Tkey key);
    public Task AddAsync(Tkey key);
    public void RemoveAsync(Tkey key);
}

