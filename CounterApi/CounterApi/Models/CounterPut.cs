using System.ComponentModel.DataAnnotations;

namespace CounterApi.Models
{
    public class CounterPut
    {
        [Required]
        public Guid Magic { get; set; }

        [Required]
        [MaxLength(200)]
        public string Key { get; set; }
    }
}
