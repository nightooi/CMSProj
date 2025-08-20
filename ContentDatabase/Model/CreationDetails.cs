using ContentDatabase.Model;

namespace ContentDatabase
{
    public abstract class CreationDetails
    {
        //Time of authoring
        public DateTime Constructed { get; set; }
        //Last time of generation to the FileSystem(ready for to Serve)
        public DateTime? Generated { get; set; }
        //Time of Publishing(First time Live)
        public DateTime Published { get; set; }
        public Guid? AuthorId { get; set; }
        public string? CopyRight { get; set; }
        public string? CopyRightDisclaimer { get; set; }
        public Author Author { get; set; }
        public DateTime LastRevisionTime { get; set; }
        public Author RevisionAuthor { get; set; }
    }
}
