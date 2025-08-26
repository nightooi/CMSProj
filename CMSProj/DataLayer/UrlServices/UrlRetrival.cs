using ContentDatabase;

namespace CMSProj.DataLayer.UrlServices
{
    public class UrlRetrival : IUrlRetrievalService
    {
        ContentContext _ctx;
        IDatalayerFactory<UrlGuidAdapter, ContentDatabase.Model.Page> _adapterFactory;

        public UrlRetrival(ContentContext ctx, IDatalayerFactory<UrlGuidAdapter, ContentDatabase.Model.Page> adapterFactory)
        {
            _ctx = ctx;
            _adapterFactory = adapterFactory;
        }
        public ICollection<UrlGuidAdapter> GetUrls()
        {
            return _ctx.Pages.SelectPublishedPageWithPublishedComponents(_adapterFactory.Create);
        }

        public async Task<ICollection<UrlGuidAdapter>> GetUrlsAsync(CancellationToken token)
        {
            return await _ctx.Pages.SelectPublishedPageWithPublishedComponentsAsync(_adapterFactory.Create);
        }
    }
}
