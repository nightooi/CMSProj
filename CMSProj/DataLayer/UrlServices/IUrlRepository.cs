using CMSProj.Controllers;

using ContentDatabase.Model;

namespace CMSProj.DataLayer.UrlServices
{
    public interface IUrlRepository
    {
        public ICollection<UrlGuidAdapter> GetUrls();
        public Task<ICollection<UrlGuidAdapter>> GetUrlsAsync(CancellationToken token);
    }
}
