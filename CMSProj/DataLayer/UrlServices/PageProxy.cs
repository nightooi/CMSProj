namespace CMSProj.DataLayer.UrlServices
{
    //pageproxy
    public class PageProxy : ContentDatabase.Model.Page
    {
        public new Guid Id { get; set; }
        public PageProxy(Guid id, string slug)
        {
            Id = id;
            Slug = slug;
        }
    }
}
