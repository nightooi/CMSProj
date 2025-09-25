using ContentDatabase;

using Microsoft.EntityFrameworkCore;
namespace CMSProj.DataLayer.DatalayerExtensions
{
    public static class ContentRetrievalExtensions
    {
        public static IQueryable<ContentDatabase.Model.PageVersion> PublishedLatestVersion(this IQueryable<ContentDatabase.Model.Page> dbModel)
        {
            return dbModel
                .Where(x => x.Published <= DateTime.UtcNow)
                .SelectMany(x => x.PageVersions)
                .Where(x => x.Published <= DateTime.UtcNow)
                .OrderByDescending(x => x.Published)
                .ThenByDescending(x => x.Version)
                .Take(1)
                .AsSplitQuery();
        }
        public static IQueryable<Guid> SlugGuidByUrl(this IQueryable<ContentDatabase.Model.PageSlug> dbModel, string url)
        {
            return dbModel.Where(x => x.Slug == url).Select(x => x.Id);
        }
        public static IQueryable<ContentDatabase.Model.PageVersion> LoadePageVersion(this IQueryable<ContentDatabase.Model.PageVersion> dbModel)
        {
            return dbModel
                .Include(x => x.PageTemplate)
                    .ThenInclude(x=> x.PageComponents)
                .Include(x => x.Components)
                    .ThenInclude(x => x.Assets)
                .Include(x => x.Components)
                    .ThenInclude(x=> x.PayLoad)
                .Include(x=> x.Components)
                .AsSplitQuery();
        }

        public static IQueryable<ContentDatabase.Model.PublishedPageSlug> LoadedTemplate(this IQueryable<ContentDatabase.Model.PublishedPageSlug> dbModel)
        {
            return dbModel
                .Include(x => x.PageVersion)
                    .ThenInclude(x => x.PageTemplate)
                        .ThenInclude(x => x.PageComponents)
                .AsSplitQuery();
        }
        public static IQueryable<ContentDatabase.Model.PublishedPageSlug> LoadePageVersion(this IQueryable<ContentDatabase.Model.PublishedPageSlug> dbModel)
        {
            return dbModel
               .Include(x => x.PageVersion)
               .Include(x => x.PageVersion)
                   .ThenInclude(x => x.Components)
               .Include(x => x.PageVersion)
                   .ThenInclude(x => x.PageTemplate)
                       .ThenInclude(x => x.PageComponents)
               .Include(x => x.PageVersion)
                   .ThenInclude(x => x.Components)
                       .ThenInclude(x => x.PayLoad)
               .Include(x => x.PageVersion)
                   .ThenInclude(x => x.Components)
                       .ThenInclude(x => x.Assets)
               .Include(x => x.Page)
                   .ThenInclude(x => x.Slug)
               .AsSplitQuery();
        }
        public static IQueryable<ContentDatabase.Model.Page> PageBySlugGuid(this IQueryable<ContentDatabase.Model.PublishedPageSlug> dbModel, Guid slugGuid)
        {
            return dbModel.Where(x => x.SlugId == slugGuid).Select(x => x.Page);
        }
        public static IQueryable<ContentDatabase.Model.PublishedPageSlug> PageVersionByPageGuid(this IQueryable<ContentDatabase.Model.PublishedPageSlug> dbModel, Guid guid)
        {
            return dbModel.Where(x => x.PageId == guid)
                .Include(x => x.PageVersion)
                    .ThenInclude(x => x.PageTemplate)
                        .ThenInclude(x => x.PageComponents);
        }
        public static async Task<IQueryable<ContentDatabase.Model.Page>> WithPublishedVersionAsync(this IQueryable<ContentDatabase.Model.Page> page, ContentContext ctx)
        {
            var slug = await page.Select(x => x.SlugId).SingleOrDefaultAsync();
            var res = await ctx.PublishedPages
                .Where(x => x.SlugId == slug)
                .Select(x => x.PageVersion.Id)
                .SingleOrDefaultAsync();
            return page.Include(x => x.PageVersions.Where(x => x.Id == res)).AsSplitQuery();
        }
        public static IQueryable<ContentDatabase.Model.PageVersion> WithAssets(this IQueryable<ContentDatabase.Model.PageVersion> comp)
        {
            return comp
                .Include(x => x.Components)
                .ThenInclude(x => x.Assets);
        }
    }
}
