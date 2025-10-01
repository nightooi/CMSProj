using System.ComponentModel.DataAnnotations;

namespace CounterApi.Data.Model
{
    public class Counter
    {
        public Guid Id { get; set; }
        [MaxLength(200)]
        public string Page { get; set; }
        public int Count { get; set; }
        public ICollection<CounterUpdate>? UpdateLog { get; set; }
    }
}
