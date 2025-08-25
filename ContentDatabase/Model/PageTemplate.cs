using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations.Schema;

namespace ContentDatabase.Model
{
    [PrimaryKey(nameof(Id))]
    public class PageTemplate : CreationDetails
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; }
        public int Version { get; set; }
        public ICollection<PageComponent> PageComponents { get; set; } = new List<PageComponent>();
        public ICollection<Page> Pages { get; set; } = new List<Page>();
    }
}
