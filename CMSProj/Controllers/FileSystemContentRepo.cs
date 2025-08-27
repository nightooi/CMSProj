
namespace CMSProj.Controllers
{
    public class FileSystemContentRepo(IContentCache contentcache) : IContentRepository
    {
        IContentCache _contentCache = contentcache;
        public IContentCache contentCache => _contentCache;

        public async Task<IEnumerable<string>> GetPageConentAsync(Guid guid)
        {
            var tsx = new CancellationTokenSource();
            try
            {
                await InitializeAsync(tsx.Token);
                return _contentCache.GetFullPage(guid);
            }
            catch(Exception exc)
            {
                tsx.Cancel();
            }
            finally
            {
                tsx.Dispose();
            }
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

        public async Task<IContentRepository> InitializeAsync(CancellationToken token)
        {
            if(contentCache is null || contentCache.Cache.Count < 1)
                await _contentCache.InitializeAsync(token);
            return this;
        }
    }
}

