using CMSProj.DataLayer.UrlServices;

using ContentDatabase;
using ContentDatabase.DeserliazationTypes;
using ContentDatabase.Model;

using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;

using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
namespace CMSProj.DataLayer
{
    public interface IContentExcavationService<T>
    {
        public T RetrieveContent(ContentDatabase.Model.Page page);
        public Task<T> RetrieveContentAsync(ContentDatabase.Model.Page page, CancellationToken token);

    }
    public interface IPublishedPageRetrieval
    {
        public ContentDatabase.Model.Page RetrievePage(string slug);
        public Task<ContentDatabase.Model.Page> RetrievePageAsync(string slug, CancellationToken? cancellationToken);
        public ContentDatabase.Model.Page RetrievePage(Guid slugGuid);
        public Task<ContentDatabase.Model.Page> RetrievePageAsync(Guid slugGuid, CancellationToken? cancellationTokan);
    }
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
    public interface IPageRepository
    {
        public ScaffoldAdapter ScaffoldPage(Guid guid);
        public Task<ScaffoldAdapter> ScaffoldPageAsync(Guid guid, CancellationToken token);
        public PageAdapter ConstructCompletePage(ScaffoldAdapter scaffoldedPage, Guid slugGuid);
        public Task<PageAdapter> ConstructCompletePageAsync(ScaffoldAdapter scaffoldedPage, Guid slugGuid, CancellationToken token);
        public Task CopyAssetsToServingDirectory(Guid guid, CancellationToken token);
    }
    public class PageRepository : IPageRepository
    {
        private IContentExcavationService<ScaffoldAdapter> ScExc { get; }
        private IContentExcavationService<ICollection<AssetAdapter>> AsstExc { get; }
        private IDatalayerFactory<PageAdapter, ContentDatabase.Model.Page> PageFact { get; }
        private IPublishedPageRetrieval PageRetrieval { get; }
        public PageRepository(
            IContentExcavationService<ScaffoldAdapter> scaffoldExc,
            IAssetLocator assetExc,
            IPublishedPageRetrieval retrieval,
            IDatalayerFactory<PageAdapter, ContentDatabase.Model.Page> pageFact)
        {
            ScExc = scaffoldExc;
            AsstExc = assetExc;
            PageFact = pageFact;
            PageRetrieval = retrieval;
        }
        public PageAdapter ConstructCompletePage(ScaffoldAdapter scaffoldedPage, Guid slugGuid)
        {
            var page = PageRetrieval.RetrievePage(slugGuid);
            var pageAdpt = PageFact.Create(page);
            MergePeripherals(pageAdpt, scaffoldedPage);
            return pageAdpt;
        }
        private PageAdapter MergePeripherals(PageAdapter adapter, ScaffoldAdapter scaffoldAdapter)
        {
           adapter.Scaffolding = scaffoldAdapter;
           adapter.HeaderContents += scaffoldAdapter.HeaderContents;
           adapter.JsContents += scaffoldAdapter.JsContents;

            foreach(var comp in adapter.PageContent)
            {
                adapter.HeaderContents += comp.HeaderContents;
                adapter.JsContents += comp.JsContents;
            }
            return adapter;
        }

        public async Task<PageAdapter> ConstructCompletePageAsync(ScaffoldAdapter scaffoldedPage, Guid slugGuid, CancellationToken token)
        {
            var page = await PageRetrieval.RetrievePageAsync(slugGuid, token);
            var pageAdpt = PageFact.Create(page);
            MergePeripherals(pageAdpt, scaffoldedPage);
            return pageAdpt;
        }

        public Task CopyAssetsToServingDirectory(Guid slugGuid, CancellationToken token)
        {
            var page = PageRetrieval.RetrievePage(slugGuid);
            return AsstExc.RetrieveContentAsync(page, token);
        }

        public ScaffoldAdapter ScaffoldPage(Guid slugGuid)
        {
            var page = PageRetrieval.RetrievePage(slugGuid);
            return ScExc.RetrieveContent(page);
        }

        public async Task<ScaffoldAdapter> ScaffoldPageAsync(Guid slugGuid, CancellationToken token)
        {
            var page = await PageRetrieval.RetrievePageAsync(slugGuid, token);
            return await ScExc.RetrieveContentAsync(page, token);
        }
    }
    public interface IDatalayerFactory<T, U>
    {
        public T Create(U model);
    }

    public class AssetFactory : IDatalayerFactory<CMSProj.DataLayer.AssetAdapter, ContentDatabase.Model.Assets>
    {

        public AssetAdapter Create(Assets model)
        {
            return new AssetAdapter(model.Id, model.Published)
            {
                Uri = model.Url,
                AssetFiletype = model.AssetFileType.FileType,
                Content = null,
                HeaderContents = null,
            };
        }
    }
    public interface IDeserializer<T>
    {
        T? Deserialize(string Origin);
    }
    public class ScaffoldAdapterFactory : IDatalayerFactory<CMSProj.DataLayer.ScaffoldAdapter, ContentDatabase.Model.PageTemplate>
    {
        public ScaffoldAdapter Create(PageTemplate model)
        {
            var peripherals = model.PageComponents.ExtractPeripheralsExt();
            var scaffold = new ScaffoldAdapter(model.Id, model.Published)
            {
                HeaderContents = $"{peripherals.HeaderContents}\n{peripherals.JsContents}",
                JsContents = $"{peripherals.JsContents}",
                ComponentHtml = [],
                RenderChildOffsets = []
            };

            model.PageComponents.ExtractMarkupAndChildOffsetsExt(scaffold);
            return scaffold;
        }
    }
    public static class AdapterExtensions
    {
        public static ICollection<TResult> ExtractAssetsExt<TIn, TResult>(this ICollection<TIn> assets, Func<TIn, TResult> factory)
        {
            var compiled = new List<TResult>();
            foreach(var asset in assets)
            {
                compiled.Add(factory(asset));
            }
            return compiled;
        }
        public static Peripherals ExtractPeripheralsExt<T>(this ICollection<T> values) where T : IComponentPeripheral
        {
            var headers = (Peripherals)(new PageAdapter(new Guid(), DateTime.UtcNow));
            StringBuilder css = new();
            StringBuilder outsideHeader = new();
            StringBuilder headerJs = new();
            StringBuilder js = new ();
            foreach(var value in values)
            {
                outsideHeader.Append(Interpolateheaders(value, value => value.OtherHeaders));
                css.Append(Interpolateheaders(value, value => value.CssHeaderTags));
                headerJs.Append(Interpolateheaders(value, value => value.JsHeaderTags));
                js.Append(Interpolateheaders(value, value => value.JsBodyTags));
            }
            outsideHeader.Append(css);
            outsideHeader.Append(headerJs);
            headers.HeaderContents = outsideHeader.ToString();
            headers.JsContents = js.ToString();
            return headers;
        }
        public static void ExtractMarkupAndChildOffsetsExt(this ICollection<PageComponent> scaffoldingComponents, ScaffoldAdapter scaffold)
        {
            var que = new Queue<string>();
            var childOff = new List<ChildOffset>();
            var offs = scaffoldingComponents.OrderByDescending(x => x.SelfPageOrder).ToArray();
            int it = 0;
            foreach(var a in scaffoldingComponents.OrderByDescending(x => x.SelfPageOrder))
            {
                que.Enqueue(a.ComponentHtml);
                childOff.Add(new ChildOffset { ComponentGuid = a.Id, RenderOffset = a.ChildOffset});
            }
            scaffold.ComponentHtml = que;
            scaffold.RenderChildOffsets = childOff;
        }
        private static string Interpolateheaders(IComponentPeripheral value, Func<IComponentPeripheral, string?> propt)
        {
            return $"{(propt(value) == null || propt(value) == string.Empty ? "" : string.Empty + $"{propt(value)}\n")}";
        }
    }
    public class PageAdapterFactory : IDatalayerFactory<CMSProj.DataLayer.PageAdapter, ContentDatabase.Model.Page>
    {
        public PageAdapter Create(ContentDatabase.Model.Page model)
        {
            return new PageAdapter(model.Id, model.Published)
            {
                Scaffolding = new(model.PageVersions.First().PageTemplate.Id, DateTime.UtcNow),
                Content = new(),
                Html = new(),
                Slug = model.Slug.Slug,
                HeaderContents = string.Empty,
                JsContents = string.Empty,
                PageName = model.PageName,
                PageContent = (IReadOnlyCollection<ContentComponent>)model.PageVersions.First().Components.ExtractAssetsExt(new ContentComponentFactory().Create),
            };
        }
    }
    
    public class ContentComponentFactory : IDatalayerFactory<CMSProj.DataLayer.ContentComponent, ContentDatabase.Model.AuthoredComponent>
    {
        public ContentComponent Create(AuthoredComponent model)
        {
            return new ContentComponent(model.Id, model.Published)
            {
                Assets = (IReadOnlyCollection<AssetAdapter>)model.Assets.ExtractAssetsExt(new AssetFactory().Create),
                Content = JsonSerializer.Deserialize<CLRComponentContent>(model.PayLoad.Content),
                Html = JsonSerializer.Deserialize<CLRComponentMarkup>(model.PayLoad.Markup),
                HeaderContents = $"{model.OtherHeaders}\n {model.CssHeaderTags}\n {model.JsHeaderTags}",
                ScaffoldAdapterId = model.PageComponentId,
                JsContents = model.JsBodyTags,
            };
        }
    }
    /// <summary>
    /// Could in the future be used to generate a serving directory on the host with finnished compiled documents.
    /// </summary>
    public class ScaffoldingRetrievalService : IContentExcavationService<ScaffoldAdapter>
    {
        private ContentContext _ctx;
        private IDatalayerFactory<ScaffoldAdapter, ContentDatabase.Model.PageTemplate> _factory;
        public ScaffoldingRetrievalService(IDatalayerFactory<ScaffoldAdapter, ContentDatabase.Model.PageTemplate> factory, ContentContext ctx)
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
        public ScaffoldAdapter RetrieveContent(ContentDatabase.Model.Page page)
        {
            AssertLoadedVersion(page)?.FirstOrDefault();
            AssertLoadedTemplate(page)?.FirstOrDefault();
            return _factory.Create(page.PageVersions.First().PageTemplate);
        }

        public async Task<ScaffoldAdapter> RetrieveContentAsync(ContentDatabase.Model.Page page, CancellationToken token)
        {
            var loadVer = AssertLoadedVersion(page);
            if (loadVer is not null) await loadVer.FirstOrDefaultAsync(token);

            var loadTemp = AssertLoadedTemplate(page);
            if (loadTemp is not null) await loadTemp.FirstOrDefaultAsync(token);

            return await Task.Run(() => _factory.Create(page.PageVersions.First().PageTemplate));
        }
    }
    public interface IAssetLocator : IContentExcavationService<ICollection<AssetAdapter>>
    {

    }
    public class PrepAssetsService : IAssetLocator
    {
        IWebHostEnvironment _env;
        ContentContext _ctx;
        IServiceProvider _srvcPrv;
        public PrepAssetsService(IWebHostEnvironment env, ContentContext ctx, IServiceProvider provider)
        {
            _env = env;
            _ctx = ctx;
            _srvcPrv = provider;
        }
        public ICollection<AssetAdapter> RetrieveContent(ContentDatabase.Model.Page page)
        {
            ContentDatabase.Model.PageVersion? version = page.PageVersions?.First();
            if(page.PageVersions?.First() is null ||
                page.PageVersions.First().Components is null ||
                page.PageVersions.First().Components.Any(x=> x.Assets == null))
            version = AssertLoadedAssets(page).SingleOrDefault();

            var adapters = CreateAdapters(version.Components);
            using var scope = _srvcPrv.CreateScope();
            EnsureExistsInOutputDirectory(adapters, scope.ServiceProvider.GetRequiredService<HttpContext>());
            return adapters;
        }
        private ICollection<AssetAdapter> CreateAdapters(ICollection<ContentDatabase.Model.AuthoredComponent> components)
        {
            var adapterList = new List<AssetAdapter>();
            foreach (var item in components)
            {
                adapterList.AddRange(CreateAssetList(item.Assets));
            }
            return adapterList;
        }
        private Task EnsureExistsInOutputDirectory(ICollection<AssetAdapter> adapter, HttpContext httpCtx)
        {
            var filecopy = new List<Task>();
            foreach(var item in adapter)
            {
                var split = item.Uri.Split("/");
                if (item.Uri == httpCtx.Request.Host.Value &&
                    File.Exists(item.Uri)                   &&
                    !_env.WebRootFileProvider.GetFileInfo($"lib/{split.Last()}").Exists)
                        filecopy.Add(Task.Run(() => File.Copy(item.Uri, Path.Combine(_env.WebRootPath, "lib", split.Last()))));
            }
            return Task.WhenAll(filecopy);
        }
        private IEnumerable<AssetAdapter> CreateAssetList(ICollection<ContentDatabase.Model.Assets> assets)
        {
            var fact = new AssetFactory();
            foreach(var asset in assets)
            {
                yield return fact.Create(asset);
            }
        }
        private IQueryable<PageVersion> AssertLoadedAssets(ContentDatabase.Model.Page page)
        {
            if (page.PageVersions?.First() is null)
                return _ctx.Entry(page).Collection(x => x.PageVersions)
                    .Query()
                    .WithAssets();

            if (page.PageVersions.First().Components is null)
                return _ctx.PageVersions
                    .Where(x => x.Id == page.PageVersions.First().Id)
                    .WithAssets()
                    .AsSplitQuery();

            return _ctx.PublishedPages!
                .PageVersionByPageGuid(page.Id)
                .Select(x=> x.PageVersion)
                .WithAssets();
        }

        public async Task<ICollection<AssetAdapter>> RetrieveContentAsync(ContentDatabase.Model.Page page, CancellationToken token)
        {
            var version = await AssertLoadedAssets(page).SingleOrDefaultAsync(token);
            if (token.IsCancellationRequested)
                return [];
            var adapters = CreateAdapters(version!.Components);
            if (token.IsCancellationRequested)
                return [];

            using var scp = _srvcPrv.CreateAsyncScope();
            await EnsureExistsInOutputDirectory(adapters, scp.ServiceProvider.GetRequiredService<HttpContext>());
            return adapters;
        }
    }
    public abstract class ComponentInfo
    {
        public Guid Id { get; protected set; }
        public DateTime Published { get; protected set; }
    }
    public abstract class CMSContent : Peripherals
    {
        public string? HeaderContents { get; set; }
        public string? JsContents { get; set; }
        public ContentDatabase.DeserliazationTypes.CLRComponentMarkup? Html { get; set; }
        public ContentDatabase.DeserliazationTypes.CLRComponentContent? Content { get; set; }
        public CMSContent(Guid guid, DateTime published)
        {
            Id = guid;
            Published = published;
        }
    }
    public abstract class Peripherals : ComponentInfo
    {
        public string? HeaderContents { get; set; }
        public string? JsContents { get; set; }
    }
    public class AssetAdapter : CMSContent
    {
        public AssetAdapter(Guid guid, DateTime published) : base(guid, published)
        {
        }

        public string Uri { get; set; }
        public string AssetFiletype { get; set; }
    }
    public class ContentComponent : CMSContent
    {
        public ContentComponent(Guid guid, DateTime published) : base(guid, published)
        {
        }
        public Guid ScaffoldAdapterId { get; set; }
        public IReadOnlyCollection<AssetAdapter>? Assets { get; set; }
    }
    public class ScaffoldAdapter : Peripherals
    {
        public Queue<string> ComponentHtml { get; set; }
        public List<ChildOffset> RenderChildOffsets { get;set; }

        public ScaffoldAdapter(Guid guid, DateTime published)
        {
            Id = guid;
            Published = published;
        }
    }
    public class ChildOffset
    {
        public Guid ComponentGuid { get; set; }
        public int RenderOffset { get; set; }
    }
    public class PageAdapter : CMSContent
    {
        public PageAdapter(Guid guid, DateTime published) : base(guid, published)
        {
        }

        public ScaffoldAdapter Scaffolding { get; set; }
        public IReadOnlyCollection<ContentComponent> PageContent { get; set; }
        public string Slug { get; set; }
        public string PageName { get; set; }
        public AssetAdapter PageIcon { get; set; }
    }
}
