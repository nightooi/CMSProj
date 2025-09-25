namespace CMSProj.DataLayer.PageServices.Components
{
    public class AssetAdapter : CMSContent
    {
        public AssetAdapter(Guid guid, DateTime published) : base(guid, published)
        {
        }

        public string Uri { get; set; }
        public string AssetFiletype { get; set; }
    }
}
