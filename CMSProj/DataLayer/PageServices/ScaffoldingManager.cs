using CMSProj.DataLayer.DatalayerExtensions;
using CMSProj.DataLayer.PageServices.AdapterFactories;
using CMSProj.DataLayer.PageServices.Components;

using ContentDatabase;
using ContentDatabase.Model;

using Microsoft.EntityFrameworkCore;
namespace CMSProj.DataLayer.PageServices
{
    /// <summary>
    /// Could in the future be used to generate a serving directory on the host with finnished compiled documents.
    /// </summary>
    public class ScaffoldingManager : IContentExcavationService<ScaffoldAdapter>
    {
        private ContentContext _ctx;
        private IDatalayerFactory<ScaffoldAdapter, PageTemplate> _factory;
        public ScaffoldingManager(IDatalayerFactory<ScaffoldAdapter, PageTemplate> factory, ContentContext ctx)
        {
            _factory = factory;
            _ctx = ctx;
        }
        private IQueryable<PageVersion>? AssertLoadedVersion(Page page)
        {
            if (page is null)
                throw new Exception("Page null???");


            if(page.PageVersions is null || page.PageVersions.FirstOrDefault() is null)
            {
                return _ctx.PublishedPages!.PageVersionByPageGuid(page.Id)
                    .Select(x => x.PageVersion);
            }
            return null;
       }
        private IQueryable<PageTemplate>? AssertLoadedTemplate(Page page)
        {
            if(page.PageVersions.First().PageTemplate is null)
            {
                return _ctx.Entry(page)
                       .Reference(x => x.PageVersions.First().PageTemplate)
                       .Query()
                       .Include(x => x.PageComponents);
            }
            return null;
        }
        public ScaffoldAdapter RetrieveContent(Page page)
        {
            AssertLoadedVersion(page)?.FirstOrDefault();
            AssertLoadedTemplate(page)?.FirstOrDefault();
            return _factory.Create(page.PageVersions.First().PageTemplate);
        }

        public async Task<ScaffoldAdapter> RetrieveContentAsync(Page page, CancellationToken token)
        {
            var loadVer = AssertLoadedVersion(page);
            if (loadVer is not null) await loadVer.FirstOrDefaultAsync(token);

            var loadTemp = AssertLoadedTemplate(page);
            if (loadTemp is not null) await loadTemp.FirstOrDefaultAsync(token);

            return await Task.Run(() => _factory.Create(page.PageVersions.First().PageTemplate));
        }
    }
}
