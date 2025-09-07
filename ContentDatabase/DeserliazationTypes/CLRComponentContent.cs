namespace ContentDatabase.DeserliazationTypes
{
    /// <summary>
    /// Meant for serializing and deserializing the the Markup and Content Fields in the ComponentMarkupType
    /// 
    /// this is mostly so we can search for content across tags
    /// </summary>
    public class CLRComponentContent
    {
        public List<ContentDef> Content { get; set; }
        public class ContentDef
        {
            public Guid Id { get; set; }
            public string  Content { get; set; }
        }
    }
}
