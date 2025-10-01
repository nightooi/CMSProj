using CMSProj.DataLayer.PageServices.AdapterFactories;
using CMSProj.DataLayer.PageServices.Components;

namespace CMSProj.DataLayer.PageServices.Repo
{
    public class PublicationManger : IPageManager
    {
        private IContentExcavationService<ScaffoldAdapter> ScExc { get; }
        private IContentExcavationService<ICollection<AssetAdapter>> AsstExc { get; }
        private IDatalayerFactory<PageAdapter, ContentDatabase.Model.Page> PageFact { get; }
        private IPublishedPageRepository PageRetrieval { get; }
        public PublicationManger(
            IContentExcavationService<ScaffoldAdapter> scaffoldExc,
            IAssetLocator assetExc,
            IPublishedPageRepository retrieval,
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
            return MergePeripherals(pageAdpt, scaffoldedPage);
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
            return MergePeripherals(pageAdpt, scaffoldedPage);
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
}
