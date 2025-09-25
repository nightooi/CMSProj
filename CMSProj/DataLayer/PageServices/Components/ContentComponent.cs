namespace CMSProj.DataLayer.PageServices.Components
{
    public class ContentComponent : CMSContent
    {
        public ContentComponent(Guid guid, DateTime published) : base(guid, published)
        {
        }
        public Guid ScaffoldAdapterId { get; set; }
        public IReadOnlyCollection<AssetAdapter>? Assets { get; set; }
    }
}
