using System.ComponentModel.DataAnnotations;

namespace ContentDatabase.Model;

public class CounterUpdate
{
    public int Id { get; set; }
    public DateTime RequestTime { get; set; }
    public Guid CounterId { get; set; }
    public Counter Counter { get; set; }
    [MaxLength(400)]
    public string LogMessage { get; set; }
}
