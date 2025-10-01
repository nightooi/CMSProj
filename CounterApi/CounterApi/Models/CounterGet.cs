using System.ComponentModel.DataAnnotations;

namespace CounterApi.Models
{
    public class CounterGet
    {
        [Required]
        public Guid Magic { get; set; }

        [Required]
        [MaxLength(200)]
        public string Key { get; set; }
    }
}
