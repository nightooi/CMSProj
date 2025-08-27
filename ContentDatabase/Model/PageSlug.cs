using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContentDatabase.Model
{
    public class PageSlug
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public Guid Id { get; set; }

        [MaxLength(1000)]
        [Required]
        [Key]public string Slug { get; set; }

        public virtual ICollection<Page> Pages { get; set; } = [];
    }
}
