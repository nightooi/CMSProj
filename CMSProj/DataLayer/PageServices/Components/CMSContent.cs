namespace CMSProj.DataLayer.PageServices.Components
{
    public abstract class CMSContent : Peripherals
    {
        public string? HeaderContents { get; set; }
        public string? JsContents { get; set; }
        public ContentDatabase.DeserliazationTypes.CLRComponentMarkup? Html { get; set; }
        public ContentDatabase.DeserliazationTypes.CLRComponentContent? Content { get; set; }
        public string HtmlMarkup { get; set; }
        public CMSContent(Guid guid, DateTime published)
        {
            Id = guid;
            Published = published;
        }
    }
}
