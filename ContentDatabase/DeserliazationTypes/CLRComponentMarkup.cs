namespace ContentDatabase.DeserliazationTypes
{
    public class CLRComponentMarkup 
    {
        public Guid Id { get; set; }
        public string Markup { get; set; }
        public List<ContentOffset> Content { get; set; }
        public class ContentOffset
        {
            public Guid ContentId { get; set; }
            public int Offset { get; set; }
        }
    }
}
