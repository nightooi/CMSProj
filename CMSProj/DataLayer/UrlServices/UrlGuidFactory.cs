namespace CMSProj.DataLayer.UrlServices
{
    public class UrlGuidFactory : IDatalayerFactory<CMSProj.DataLayer.UrlServices.UrlGuidAdapter, PageProxy> 
    {
        public UrlGuidAdapter Create(PageProxy page)
        {
            return new UrlGuidAdapter()
            {
                Guid = page.Id,
                PageUrl = page.Slug
            };
        }
    }
}
