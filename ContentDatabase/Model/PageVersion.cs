using System.ComponentModel.DataAnnotations.Schema;

namespace ContentDatabase.Model
{
    public class PageVersion : CreationDetails
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; }
        [ForeignKey(nameof(Page.Id))]
        public Guid PageId { get; set; }
        public int VersionNumber { get; set; }
        public Page Page { get; set; } = null!;
        public ICollection<AuthoredComponent> Components { get; set; } = new List<AuthoredComponent>();
    }
}
