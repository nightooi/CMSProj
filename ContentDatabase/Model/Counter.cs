namespace ContentDatabase.Model;

public class Counter
{
    public Guid Id { get;set; }
    public string Page { get; set; }
    public int Count { get; set; }
    public ICollection<CounterUpdate>? UpdateLog { get; set; }
}
