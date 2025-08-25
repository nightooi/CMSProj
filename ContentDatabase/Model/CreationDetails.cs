using System.ComponentModel.DataAnnotations.Schema;

namespace ContentDatabase.Model
{
    public abstract class CreationDetails
    {
        //Time of authoring
        public DateTime Constructed { get; set; }
        //Last time of generation to the FileSystem(ready for to Serve)
        public DateTime? Generated { get; set; }
        //Time of Publishing(First time Live)
        [ForeignKey(nameof(CreationAuthor.Id))]
        public Guid RevisionAuthorId { get; set; }
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
}
