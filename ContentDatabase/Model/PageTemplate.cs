using Microsoft.EntityFrameworkCore;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContentDatabase.Model
{
    [PrimaryKey(nameof(Id))]
    public class PageTemplate : CreationDetails, Id
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; }
        public int Version { get; set; }
        public ICollection<PageComponent> PageComponents { get; set; } = [];
        public ICollection<Page> Pages { get; set; } = [];
        public ICollection<PageVersion> PageVersions = [];
    }
}
