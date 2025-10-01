using CMSProj.DataLayer.UrlServices;
using CMSProj.SubSystems.BackGroundServices.UrlUpdate;

using System.Text.RegularExpressions;
/// <summary>
/// Cache is builtin... which is rather unfortunate, caching functionality is a dependency which should be extracted 
/// into it's own type.
/// </summary>
public class ActiveRoutesManager : IActiveRouteManager, IPostActivator<IActiveRouteManager>
{
    Dictionary<string, Guid> _routes;
    IRouteMatcherFactory routeMatcherFactory;

    //At home hot cache
    IOrderedEnumerable<string> _existingRoutes;

    IUrlRepository UrlRetrievalService;
    Task<ICollection<UrlGuidAdapter>> _currentUpdate;
    Task<IOrderedEnumerable<string>> _updatingRoutesCacheTask;
    IUpdateWorkResult<WorkerResult<int>> ResultOrchestrator;
    object _lock = new();
    ILogger<ActiveRoutesManager> Logger { get; set; }

    public ActiveRoutesManager(
        IRouteMatcherFactory routeMatcher,
        IUrlRepository urlRetrievalService,
        IUpdateWorkResult<WorkerResult<int>> updateResult,
        ILogger<ActiveRoutesManager> logger)
    {
        UrlRetrievalService = urlRetrievalService;
        ResultOrchestrator = updateResult;
        Logger = logger;
        _routes = new();
        routeMatcherFactory = routeMatcher;
    }
    public void GetAvailableRoutes()
    {
        var res = UrlRetrievalService.GetUrls();
        AddUrls(res);
    }
    private void AddUrls(ICollection<UrlGuidAdapter> urlCollection)
    {
        _routes.Clear();
        foreach(var keyvalue in urlCollection)
        {
            if (!_routes.TryAdd(keyvalue.PageUrl, keyvalue.Guid))
                _routes[keyvalue.PageUrl] = keyvalue.Guid;
        }
    }
    public async Task GetAvailableRoutesAsync(CancellationToken token)
    {
        _currentUpdate = UrlRetrievalService.GetUrlsAsync(token);
        var res = await _currentUpdate;
        ResultOrchestrator.UpdateWorkState(this, WorkerState.Sorting, LogLevel.Information);
        ///this will probably land me in jail, alas i do not have time to implmenet a hot cache.
        try
        {
            if (!token.IsCancellationRequested)
            {
                lock (_lock)
                {
                    AddUrls(res);
                }
                _updatingRoutesCacheTask = Task.Run<IOrderedEnumerable<string>>(_routes.Keys.Order, token);
                _existingRoutes = await _updatingRoutesCacheTask;
            }
            ResultOrchestrator.UpdateWorkState(this, WorkerState.MergingManager, LogLevel.Information);
        }
        catch(Exception exc)
        {
            ResultOrchestrator.UpdateWorkState(this, WorkerState.MergingManager, LogLevel.Error);
            Logger.Log(LogLevel.Error, $"{exc.Message} \n {exc.InnerException.Message}");
        }
        finally
        {
            _currentUpdate.Dispose();
            _updatingRoutesCacheTask.Dispose();
        }
    }

    public Guid? GetPageGuid(string? route)
    {
        var definedUrl = MatchRoute(route);
        if (definedUrl is null)
            return null;

        Guid guid;
        if (!_routes.TryGetValue(definedUrl, out guid))
            return null;
        return guid;
    }
    private string? MatchRoute(string? route)
    {
        if (route is null || route == string.Empty)
            return "Home";

        Regex regex = routeMatcherFactory
            .Create(route);
        //fastest way of making sure we're not running regex on all defined routes
        var range = _existingRoutes.Where(x => x.StartsWith(route.Substring(0, 1)));
        foreach (var defined in range)
        {
            if (regex.IsMatch(defined))
                return defined;
        }
        return null;
    }
    public async Task<Guid?> GetPageGuidAsync(string? route, CancellationToken token)
    {
        if (_routes.Keys.Count < 1)
        {
            await InitializeAsync(token);
            return GetPageGuid(route);
        }

        if(_currentUpdate is not null)
        await _currentUpdate;

        return GetPageGuid(route);
    }

    public IActiveRouteManager Initialize()
    {
        GetAvailableRoutes();
        return this;
    }

    public async Task<IActiveRouteManager> InitializeAsync(CancellationToken token)
    {
        await GetAvailableRoutesAsync(token);
        return this;
    }
}

