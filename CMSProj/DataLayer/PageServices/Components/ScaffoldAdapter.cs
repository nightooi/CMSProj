namespace CMSProj.DataLayer.PageServices.Components
{
    public class ScaffoldAdapter : Peripherals
    {
        public Queue<ScaffoldingItem> ComponentHtml { get; set; }

        public ScaffoldAdapter(Guid guid, DateTime published)
        {
            Id = guid;
            Published = published;
        }
    }
    public record ScaffoldingItem(Guid Guid, string html, ChildOffset offset);
}
