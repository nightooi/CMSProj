namespace CMSProj.DataLayer.UrlServices
{
    //pageproxy
    public class SlugProxy : ContentDatabase.Model.PageSlug
    {
        public new Guid Id { get; set; }
        public SlugProxy(Guid id, string slug)
        {
            Id = id;
            Slug = slug;
        }
    }
}
