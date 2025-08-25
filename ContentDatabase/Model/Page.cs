using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContentDatabase.Model
{
    [PrimaryKey(nameof(Id))]
    public class Page : CreationDetails
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; }
        [MaxLength(2000)]
        public string Slug { get; set; }
        [MaxLength(200)]
        [Key] public string PageName { get; set; }
        public PageTemplate PageTemplate { get; set; }
        public ICollection<PageVersion>? PageComponenets { get; set; } = new List<PageVersion>();
    }
}
