using CMSProj.SubSystems.RouteResolvers;

namespace CMSProj.Controllers
{
    public class ContentCache(IWebHostEnvironment env, IRouteRepository routeRepository) : IContentCache
    {
        IRouteRepository _routeRepository = routeRepository;
        Dictionary<Guid, IEnumerable<string>> _cachedPages = new();
        IWebHostEnvironment _env = env;

        public IReadOnlyDictionary<Guid, IEnumerable<string>> Cache => (IReadOnlyDictionary<Guid, IEnumerable<string>>)_cachedPages;

        async Task<IEnumerable<string>?>? ExcavatePage(Guid guid)
        {
            var page = guid.FindPageByGuid(_env, FileMode.Open);
            string? currentLine = "";
            List<string> total = [];
            if (page is null)
                return null;

            using (var reader = new StreamReader(page))
                while ((currentLine = await reader.ReadLineAsync()) is not null)
                    total.Add(currentLine);
            return total;
        }

        public async Task AddAsync(Guid key)
        {
            var pagecontent = ExcavatePage(key);
            if(pagecontent is not null && await pagecontent is not null)
            _cachedPages.Add(key, await pagecontent);
        }

        public async Task<IEnumerable<string>?> FindAsync(Guid key)
        {
            IEnumerable<string> pageContent;
            Stream? pageStream = null;

            if ((pageContent = _cachedPages[key]) is not null)
                return pageContent;

            if ((pageStream = key.FindPageByGuid(_env, FileMode.Open)) is null)
                return null;

            await AddAsync(key);

            return _cachedPages[key];
        }

        public IEnumerable<string>? GetFullPage(Guid key)
        {
            IEnumerable<string> pageContent = [];
            if(_cachedPages.TryGetValue(key, out pageContent))
            {
                return pageContent;
            }
            return null;
        }

        public void RemoveAsync(Guid key)
        {
            Stream? stream = null;

            if (_cachedPages.ContainsKey(key))
                _cachedPages.Remove(key);

            if ((stream = key.FindPageByGuid(_env, FileMode.Open)) is not null)
            {
                stream.Close();
                File.Delete(key.GetPathByBuid(_env));
            }
        }
        public IContentCache Initialize()
        {
            var routes = _routeRepository.GetAvailableRoutes();
            foreach(var guid in routes)
            {
                AddAsync(guid);
            }
            return this;
        }

        public async Task<IContentCache> InitializeAsync(CancellationToken token)
        {
            var routes = _routeRepository.GetAvailableRoutesAsync(token);
            foreach(var guid in routes)
            {
                await AddAsync(guid);
            }
            return this;
 
        }
    }
}

