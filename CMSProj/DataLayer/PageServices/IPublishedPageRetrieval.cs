namespace CMSProj.DataLayer.PageServices
{
    public interface IPublishedPageRetrieval
    {
        public ContentDatabase.Model.Page RetrievePage(string slug);
        public Task<ContentDatabase.Model.Page> RetrievePageAsync(string slug, CancellationToken? cancellationToken);
        public ContentDatabase.Model.Page RetrievePage(Guid slugGuid);
        public Task<ContentDatabase.Model.Page> RetrievePageAsync(Guid slugGuid, CancellationToken? cancellationTokan);
    }
}
