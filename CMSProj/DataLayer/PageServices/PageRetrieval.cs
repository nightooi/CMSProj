using CMSProj.DataLayer.DatalayerExtensions;

using ContentDatabase;

using Microsoft.EntityFrameworkCore;
namespace CMSProj.DataLayer.PageServices
{
    public class PageRetrieval : IPublishedPageRetrieval
    {
        ContentContext Ctx { get; }
        public PageRetrieval(ContentContext ctx)
        {
            Ctx = ctx;
        }
        public ContentDatabase.Model.Page? RetrievePage(string slug)
        {
            var guid = Ctx.PageSlugs.SlugGuidByUrl(slug).SingleOrDefault();
            return RetrieveBySlug(guid).Result;
        }

        public ContentDatabase.Model.Page? RetrievePage(Guid slugGuid)
        {
            return RetrieveBySlug(slugGuid).Result;
        }

        public async Task<ContentDatabase.Model.Page?> RetrievePageAsync(string slug, CancellationToken? cancellationToken)
        {
            var guid = await Ctx.PageSlugs.SlugGuidByUrl(slug).SingleOrDefaultAsync(cancellationToken.Value);
            if (cancellationToken is not null)
            {
                return await RetrieveBySlug(guid);
            }
            return await RetrieveBySlug(guid);
 
        }

        public async Task<ContentDatabase.Model.Page?> RetrievePageAsync(Guid slugGuid, CancellationToken? cancellationToken)
        {
            if (cancellationToken is not null)
                return await RetrieveBySlug(slugGuid, cancellationToken.Value);

            return await RetrieveBySlug(slugGuid, cancellationToken);
        }

        private async Task<ContentDatabase.Model.Page?> RetrieveBySlug(Guid slug, CancellationToken? token)
        {
            if(token is not null)
            {
                await Ctx.PublishedPages
                    .Where(x => x.SlugId == slug)
                    .LoadePageVersion()
                    .LoadAsync(token.Value);
                return await Ctx.PublishedPages.PageBySlugGuid(slug).SingleOrDefaultAsync(token.Value);
            }
            return await RetrieveBySlug(slug);
       }
        private async Task<ContentDatabase.Model.Page?> RetrieveBySlug(Guid slug) 
        {
                await Ctx.PublishedPages
                    .Where(x => x.SlugId == slug)
                    .LoadePageVersion()
                    .LoadAsync();
            return await Ctx.PublishedPages.PageBySlugGuid(slug).SingleOrDefaultAsync();
        }

    }
}
