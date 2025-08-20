
namespace CMSProj.Controllers
{
    public class FileSystemContentRepo(IContentCache contentcache) : IContentRepository
    {
        IContentCache _contentCache = contentcache;
        public IContentCache contentCache => _contentCache;

        public async Task<IEnumerable<string>> GetPageConentAsync(Guid guid)
        {
            await InitializeAsync();
            return _contentCache.GetFullPage(guid);
        }

        /// <summary>
        /// #TODO: set up page document name injection with options pattern
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="guid"></param>
        /// <returns></returns>
        public IEnumerable<string>? GetPageContent(Guid guid)
        {
            Initialize();
            return _contentCache.GetFullPage(guid);
        }

        public IContentRepository Initialize()
        {
            if(contentCache is null || contentCache.Cache.Count < 1)
                _contentCache.Initialize();
            return this;
        }

        public async Task<IContentRepository> InitializeAsync()
        {
            if(contentCache is null || contentCache.Cache.Count < 1)
                await _contentCache.InitializeAsync();
            return this;
        }
    }
}

