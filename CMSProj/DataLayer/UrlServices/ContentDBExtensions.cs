using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

using System.Linq.Expressions;

namespace CMSProj.DataLayer.UrlServices
{
    public static class ContentDBExtensions
    {
        public static ICollection<UrlGuidAdapter> SelectPublishedPages(this DbSet<ContentDatabase.Model.Page> pages,
            Func<PageProxy, UrlGuidAdapter> transformer)
        {
            var res = pages
                 .Where(x => x.Published <= DateTime.UtcNow)
                 .Select(x => new { guid = x.Id, url = x.Slug })
                 .ToList();
            return res.Select(x => transformer(new PageProxy(x.guid, x.url))).ToList();
        }
        public static ICollection<UrlGuidAdapter> SelectPublishedPageWithPublishedComponents(this DbSet<ContentDatabase.Model.Page> pages,
           Func<PageProxy, UrlGuidAdapter> transformer)
        {
            var res = pages
                .Where(x=> x.Published <= DateTime.UtcNow && x.PageVersions
                .All(x=> x.Components
                .All(x=> x.Published <= DateTime.UtcNow)))
                .OrderBy(x=> x.PageVersions.Select(x=> x.Version))
                .Take(1)
                .Select(x=> new {guid = x.Id, url = x.Slug})
                .ToList();
            return res.Select(x => transformer(new PageProxy(x.guid, x.url))).ToList();
        }
        public static IQueryable<TResult> SelectPublishedPageWithPublishedComponents<TResult>(this DbSet<ContentDatabase.Model.Page> pages,
            Expression<Func<ContentDatabase.Model.Page, TResult>> selector)
        {
            return pages
                 .Include(x => x.PageComponenets)
                 .Where(x => x.Published <= DateTime.UtcNow && x.PageComponenets.All(x => x.Published <= DateTime.UtcNow))
                 .Select(selector);
        }
        public static async Task<ICollection<UrlGuidAdapter>> SelectPublishedPageWithPublishedComponentsAsync(this DbSet<ContentDatabase.Model.Page> pages,
            Func<PageProxy, UrlGuidAdapter> transformer)
        {
            var res = await pages
                .SelectPublishedPageWithPublishedComponents(x => new { id = x.Id, slug = x.Slug })
                .ToListAsync();

            return res.Select(x => transformer(new PageProxy(x.id, x.slug)))
                .ToList();
        }

    }
}
