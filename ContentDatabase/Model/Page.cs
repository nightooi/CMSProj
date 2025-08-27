using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContentDatabase.Model
{
    [PrimaryKey(nameof(Id))]
    public class Page : CreationDetails, Id
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; }
        [ForeignKey(nameof(PageSlug.Id))]
        public Guid SlugId { get; set; }
        public PageSlug Slug { get; set; }
        [MaxLength(1000)]
        public string PageName { get; set; }
        public ICollection<PageVersion>? PageVersions { get; set; } = new List<PageVersion>();
    }
}
