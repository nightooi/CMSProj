namespace CMSProj.DataLayer.UrlServices
{
    public interface IUrlRetrievalService
    {
        public ICollection<UrlGuidAdapter> GetUrls();
        public Task<ICollection<UrlGuidAdapter>> GetUrlsAsync(CancellationToken token);
    }
}
