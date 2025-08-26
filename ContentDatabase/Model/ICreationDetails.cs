namespace ContentDatabase.Model
{
    public interface ICreationDetails
    {
        //Time of authoring
        public DateTime Constructed { get; set; }
        //Last time of generation to the FileSystem(ready for to Serve)
        public DateTime? Generated { get; set; }
        //Time of Publishing(First time Live)
        public Guid RevisionAuthorId { get; set; }
        public DateTime Published { get; set; }
        public Guid? AuthorId { get; set; }
        public string? CopyRight { get; set; }
        public string? CopyRightDisclaimer { get; set; }
        public Author CreationAuthor { get; set; }
        public DateTime LastRevisionTime { get; set; }
        public string? RevisionDiff { get; set; }
        public ICollection<Author> RevisionAuthor { get; set; }
    }
}
