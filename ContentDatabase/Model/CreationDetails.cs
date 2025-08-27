using System.ComponentModel.DataAnnotations.Schema;

namespace ContentDatabase.Model
{
    public abstract class CreationDetails : ICreationDetails
    {
        public DateTime Constructed { get; set; }
        public DateTime? Generated { get; set; }
        [ForeignKey(nameof(CreationAuthor.Id))]
        public Guid CreationAuthorId { get; set; }
        public DateTime Published { get; set; }
        public Guid? AuthorId { get; set; }
        public string? CopyRight { get; set; }
        public string? CopyRightDisclaimer { get; set; }
        public Author CreationAuthor { get; set; }
        public DateTime LastRevisionTime { get; set; }
        public string? RevisionDiff { get; set; }
        [NotMapped]
        public ICollection<Author> RevisionAuthor { get; set; }
    }
    public interface IVersionable
    {
        public int Version { get; set; }
    }
}
