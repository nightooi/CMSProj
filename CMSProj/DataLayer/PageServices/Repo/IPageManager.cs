using CMSProj.DataLayer.PageServices.Components;

namespace CMSProj.DataLayer.PageServices.Repo
{
    public interface IPageManager
    {
        public ScaffoldAdapter ScaffoldPage(Guid guid);
        public Task<ScaffoldAdapter> ScaffoldPageAsync(Guid guid, CancellationToken token);
        public PageAdapter ConstructCompletePage(ScaffoldAdapter scaffoldedPage, Guid slugGuid);
        public Task<PageAdapter> ConstructCompletePageAsync(ScaffoldAdapter scaffoldedPage, Guid slugGuid, CancellationToken token);
        public Task CopyAssetsToServingDirectory(Guid guid, CancellationToken token);
    }
}
