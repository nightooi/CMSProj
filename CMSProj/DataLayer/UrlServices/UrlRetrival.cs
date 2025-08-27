using ContentDatabase;

using Microsoft.EntityFrameworkCore;

namespace CMSProj.DataLayer.UrlServices
{
    public class UrlRetrival : IUrlRetrievalService
    {
        ContentContext _ctx;
        IDatalayerFactory<UrlGuidAdapter, ContentDatabase.Model.PageSlug> _adapterFactory;

        public UrlRetrival(ContentContext ctx, IDatalayerFactory<UrlGuidAdapter, ContentDatabase.Model.PageSlug> adapterFactory)
        {
            _ctx = ctx;
            _adapterFactory = adapterFactory;
        }
        public ICollection<UrlGuidAdapter> GetUrls()
        {
            return _ctx.PageSlugs.ToList().Select(x => _adapterFactory.Create(x)).ToList();
        }

        public async Task<ICollection<UrlGuidAdapter>> GetUrlsAsync(CancellationToken token)
        {
            return (await _ctx.PageSlugs.ToListAsync()).Select(x => _adapterFactory.Create(x)).ToList();
        }
    }
}
