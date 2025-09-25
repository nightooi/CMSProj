using CMSProj.DataLayer.PageServices.AdapterFactories;

using ContentDatabase.Model;

namespace CMSProj.DataLayer.UrlServices.Factories
{
    public class UrlGuidFactory : IDatalayerFactory<UrlGuidAdapter, ContentDatabase.Model.PageSlug> 
    {
        public UrlGuidAdapter Create(PageSlug page)
        {
            return new UrlGuidAdapter()
            {
                Guid = page.Id,
                PageUrl = page.Slug
            };
        }
    }
}
