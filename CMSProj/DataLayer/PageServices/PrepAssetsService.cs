using CMSProj.DataLayer.DatalayerExtensions;
using CMSProj.DataLayer.PageServices.AdapterFactories;
using CMSProj.DataLayer.PageServices.Components;

using ContentDatabase;
using ContentDatabase.Model;

using Microsoft.EntityFrameworkCore;
namespace CMSProj.DataLayer.PageServices
{
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
        public ICollection<AssetAdapter> RetrieveContent(Page page)
        {
            PageVersion? version = page.PageVersions?.First();
            if(page.PageVersions?.First() is null ||
                page.PageVersions.First().Components is null ||
                page.PageVersions.First().Components.Any(x=> x.Assets == null))
            version = AssertLoadedAssets(page).SingleOrDefault();

            var adapters = CreateAdapters(version.Components);
            using var scope = _srvcPrv.CreateScope();
            EnsureExistsInOutputDirectory(adapters, scope.ServiceProvider.GetRequiredService<HttpContext>());
            return adapters;
        }
        private ICollection<AssetAdapter> CreateAdapters(ICollection<AuthoredComponent> components)
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
        private IEnumerable<AssetAdapter> CreateAssetList(ICollection<Assets> assets)
        {
            var fact = new AssetFactory();
            foreach(var asset in assets)
            {
                yield return fact.Create(asset);
            }
        }
        private IQueryable<PageVersion> AssertLoadedAssets(Page page)
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

        public async Task<ICollection<AssetAdapter>> RetrieveContentAsync(Page page, CancellationToken token)
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
}
