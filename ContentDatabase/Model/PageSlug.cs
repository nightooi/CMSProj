using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContentDatabase.Model
{
    //this has to have a composite key of Slug And a Page -> there should then be a constraint on PublishedPageslugs table to only
    //include that composite key to to the slugkey. (ensure that the page is tied to this slug.)
    [PrimaryKey(nameof(Id))]
    public class PageSlug
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get;}

        [MaxLength(1000)]
        [Required]
        [Key]public string Slug { get; set; }

        public virtual ICollection<Page>? Pages { get; set; } = [];
        public PublishedPageSlug? Pageslug { get; set; } = null!;
    }
}
