using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContentDatabase.Model
{
    [PrimaryKey(nameof(Id))]
    public class Author
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; }
        public string Name { get; set; }
        [MaxLength(300)]
        public string ContactEmail { get; set; }
        public ICollection<PageTemplate> PageTemplates { get; set; } = null!;
        public ICollection<PageComponent> PageComponents { get; set; } = null!;
    }
}
