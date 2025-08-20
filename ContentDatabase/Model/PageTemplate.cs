using System.ComponentModel.DataAnnotations.Schema;

namespace ContentDatabase.Model
{
    public class PageTemplate : CreationDetails
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; }
        [ForeignKey(nameof(Author.Id))]
        public Guid AuthorId { get; }
        public Author Author { get; set; } = new();
        public int Version { get; set; }
        public ICollection<PageComponent> PageComponents { get; set; } = new List<PageComponent>();
        public ICollection<Page> Pages { get; set; } = new List<Page>();
    }
}
