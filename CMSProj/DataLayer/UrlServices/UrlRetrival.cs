using CMSProj.DataLayer.PageServices.AdapterFactories;

using ContentDatabase;

using Microsoft.EntityFrameworkCore;

namespace CMSProj.DataLayer.UrlServices
{
    public class UrlRetrival : IUrlRetrievalService
    {
        IServiceScopeFactory _provider;
        IDatalayerFactory<UrlGuidAdapter, ContentDatabase.Model.PageSlug> _adapterFactory;

        public UrlRetrival(IServiceScopeFactory scopeFact, 
            IDatalayerFactory<UrlGuidAdapter, ContentDatabase.Model.PageSlug> adapterFactory)
        {
            _provider = scopeFact;
            _adapterFactory = adapterFactory;
        }
        public ICollection<UrlGuidAdapter> GetUrls()
        {
            using var scope = _provider.CreateScope();
            var _ctx = scope.ServiceProvider.GetRequiredService<ContentContext>();
            return _ctx.PageSlugs.ToList().Select(x => _adapterFactory.Create(x)).ToList();
        }

        public async Task<ICollection<UrlGuidAdapter>> GetUrlsAsync(CancellationToken token)
        {
            using (var scope = _provider.CreateAsyncScope())
            {
                var _ctx = scope.ServiceProvider.GetRequiredService<ContentContext>();
                return (await _ctx.PageSlugs.ToListAsync()).Select(x => _adapterFactory.Create(x)).ToList();
            }
        }
    }
}
