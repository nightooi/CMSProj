namespace CMSProj.DataLayer.PageServices.Components
{
    public class PageAdapter : CMSContent
    {
        public PageAdapter(Guid guid, DateTime published) : base(guid, published)
        {
        }

        public ScaffoldAdapter Scaffolding { get; set; }
        public IReadOnlyCollection<ContentComponent> PageContent { get; set; }
        public string Slug { get; set; }
        public string PageName { get; set; }
        public AssetAdapter PageIcon { get; set; }
    }
}
