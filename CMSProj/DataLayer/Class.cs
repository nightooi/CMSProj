using CMSProj.DataLayer.UrlServices;

using ContentDatabase;
using ContentDatabase.Model;

using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;

using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
namespace CMSProj.DataLayer
{
    public interface IContentRetrievalService<T>
    {
        public T RetrieveContent(Guid guid);
        public Task<T> RetrieveContentAsync(Guid guid);

        public ICollection<T> RetrievePublishedOnContent(DateTime dateTime);
        public Task<ICollection<T>> RetrieveContentPublishedOnAsync(DateTime dateTime);

        public ICollection<T> RetrievePublishedBy(DateTime dateTime);
        public Task<ICollection<T>> RetrievePublishedByAsync(DateTime dateTime);

        public T RetrieveComponent(Guid guid);
        public Task<T> RetrieveComponentAsync(Guid guid);
    }

    public interface IPageRetrieval
    {
        public ContentDatabase.Model.Page RetrievePage(string slug);
        public Task<ContentDatabase.Model.Page> RetrievePageAsync(string slug, CancellationToken? cancellationToken);
        public ContentDatabase.Model.Page RetrievePage(Guid slugGuid);
        public Task<ContentDatabase.Model.Page> RetrievePageAsync(Guid slugGuid, CancellationToken? cancellationTokan);
    }
    public class PageRepoSitory : IPageRetrieval
    {
        ContentContext Ctx { get; }
        public PageRepoSitory(ContentContext ctx)
        {
            Ctx = ctx;
        }
        public ContentDatabase.Model.Page? RetrievePage(string slug)
        {
            var guid = Ctx.PageSlugs.SlugGuidByUrl(slug).SingleOrDefault();
            return RetrieveBySlug(guid).SingleOrDefault();
        }

        public ContentDatabase.Model.Page? RetrievePage(Guid slugGuid)
        {
            return RetrieveBySlug(slugGuid).SingleOrDefault();
        }

        public async Task<ContentDatabase.Model.Page?> RetrievePageAsync(string slug, CancellationToken? cancellationToken)
        {
            var guid = await Ctx.PageSlugs.SlugGuidByUrl(slug).SingleOrDefaultAsync(cancellationToken.Value);
            if (cancellationToken is not null)
            {
                return await RetrieveBySlug(guid).SingleOrDefaultAsync(cancellationToken.Value);
            }
            return await RetrieveBySlug(guid).SingleOrDefaultAsync();
 
        }

        public Task<ContentDatabase.Model.Page?> RetrievePageAsync(Guid slugGuid, CancellationToken? cancellationToken)
        {
            if(cancellationToken is not null)
               return RetrieveBySlug(slugGuid)
                    .SingleOrDefaultAsync(cancellationToken.Value);

            return RetrieveBySlug(slugGuid)
                .SingleOrDefaultAsync();
        }

        private IQueryable<ContentDatabase.Model.Page> RetrieveBySlug(Guid slug)
        {
            return Ctx.PublishedPages
                .Include(x=> x.PageVersion)
                .LoadePageVersion()
                .PageBySlugGuid(slug);
        }
    }
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
                .AsSplitQuery();
        }
        public static IQueryable<ContentDatabase.Model.PulishedPageSlug> LoadePageVersion(this IQueryable<ContentDatabase.Model.PulishedPageSlug> dbModel)
        {
            return dbModel
                .Include(x => x.PageVersion)
                    .ThenInclude(x=> x.PageTemplate)
                        .ThenInclude(x=> x.PageComponents)
                .Include(x => x.PageVersion)
                    .ThenInclude(x => x.Components)
                        .ThenInclude(x=> x.PayLoad)
                .Include(x=> x.PageVersion)
                    .ThenInclude(x=> x.Components)
                        .ThenInclude(x=> x.Assets)
                .AsSplitQuery();
        }
        public static IQueryable<ContentDatabase.Model.Page> PageBySlugGuid(this IQueryable<ContentDatabase.Model.PulishedPageSlug> dbModel, Guid slugGuid)
        {
            return dbModel.Where(x => x.SlugId == slugGuid).Select(x => x.Page);
        }
    }

    public interface IPageOrchestratorService
    {
        //Idea is to start excavate Scaffold first and construct an empty page
        // and immiedately send it to the client
        public Page ConstructCompletePage(PageScaffold scaffoldedPage);
        public Task<Page> ConstructCompletePageAsync(PageScaffold scaffoldedPage);
        public Page ScaffoldPage(Guid guid);
        public Task<Page> ScaffoldPageAsync(Guid guid);
        public Page ScaffoldedPage(string url);
        public Task<Page> ScaffoldedPageAsync(string url);
        public Page ConstructedCompletePage(string url);
        public Task<Page> ConstructedCompletePageAsync(string url);
    }

    public interface IDatalayerFactory<T, U>
    {
        public T Create(U model);
    }

    public class AssetFactory : IDatalayerFactory<CMSProj.DataLayer.Asset, ContentDatabase.Model.Assets>
    {
        public Asset Create(Assets model)
        {
            throw new NotImplementedException();
        }
    }

    public class ScaffoldedPageFactory : IDatalayerFactory<CMSProj.DataLayer.Page, ContentDatabase.Model.PageTemplate>
    {
        public Page Create(PageTemplate model)
        {
            throw new NotImplementedException();
        }
    }
    public class PageFactory : IDatalayerFactory<CMSProj.DataLayer.Page, ContentDatabase.Model.Page>
    {
        public Page Create(ContentDatabase.Model.Page model)
        {
            throw new NotImplementedException();
        }
    }

    public class ContentComponentFactory : IDatalayerFactory<CMSProj.DataLayer.ContentComponent, ContentDatabase.Model.AuthoredComponent>
    {
        public ContentComponent Create(AuthoredComponent model)
        {
            throw new NotImplementedException();
        }
    }

    public class ScaffoldingRetrievalService : IContentRetrievalService<PageScaffold>
    {
        public PageScaffold RetrieveComponent(Guid guid)
        {
            throw new NotImplementedException();
        }

        public Task<PageScaffold> RetrieveComponentAsync(Guid guid)
        {
            throw new NotImplementedException();
        }

        public PageScaffold RetrieveContent(Guid guid)
        {
            throw new NotImplementedException();
        }

        public Task<PageScaffold> RetrieveContentAsync(Guid guid)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<PageScaffold>> RetrieveContentPublishedOnAsync(DateTime dateTime)
        {
            throw new NotImplementedException();
        }

        public ICollection<PageScaffold> RetrievePublishedBy(DateTime dateTime)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<PageScaffold>> RetrievePublishedByAsync(DateTime dateTime)
        {
            throw new NotImplementedException();
        }

        public ICollection<PageScaffold> RetrievePublishedOnContent(DateTime dateTime)
        {
            throw new NotImplementedException();
        }
    }
    public class AssetRetrievalService : IContentRetrievalService<Asset>
    {
        private ContentContext ctx { get; }
        public AssetRetrievalService(ContentContext context)
        {
            ctx = context;
        }
        public Asset RetrieveComponent(Guid guid)
        {
            throw new NotImplementedException();
        }

        public Task<Asset> RetrieveComponentAsync(Guid guid)
        {
            throw new NotImplementedException();
        }

        public Asset RetrieveContent(Guid guid)
        {
            throw new NotImplementedException();
        }

        public Task<Asset> RetrieveContentAsync(Guid guid)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Asset>> RetrieveContentPublishedOnAsync(DateTime dateTime)
        {
            throw new NotImplementedException();
        }

        public ICollection<Asset> RetrievePublishedBy(DateTime dateTime)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Asset>> RetrievePublishedByAsync(DateTime dateTime)
        {
            throw new NotImplementedException();
        }

        public ICollection<Asset> RetrievePublishedOnContent(DateTime dateTime)
        {
            throw new NotImplementedException();
        }
    }
    public abstract class CMSContent
    {
        public Guid Guid { get; }
        public ContentDatabase.DeserliazationTypes.CLRComponentMarkup Html { get; set; }
        public ContentDatabase.DeserliazationTypes.CLRComponentContent Content { get; set; }
        public DateTime PublishDate { get; }
    }
    public class Asset : CMSContent
    {
        public string Uri { get; }
        public string AssetFiletype { get; }
    }

    public class ContentComponent : CMSContent
    {
        public IReadOnlyCollection<Asset>? Assets { get; }
    }
    public class PageScaffold : CMSContent
    {
        public string Slug { get; }
        public string PageName { get; }
        public Asset PageIcon { get; }
    }
    public class Page : CMSContent
    {
        public PageScaffold Scaffolding { get; }
        public IReadOnlyCollection<ContentComponent> PageContent { get; }
    }
}
