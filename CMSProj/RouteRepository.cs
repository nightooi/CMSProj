using System.Text.RegularExpressions;

public class RouteRepository : IRouteRepository, IPostActivator<IRouteRepository>
{
    const string delimiter = "&&";
    Dictionary<string, Guid> _routes;
    IRouteMatcherFactory routeMatcherFactory;
    IReadOnlyDictionary<string, Guid> Routes => _routes;
    IReadOnlyCollection<string> ExistingRoutes => _routes.Keys;
    IWebHostEnvironment _env;
    public RouteRepository(IWebHostEnvironment env, IRouteMatcherFactory routeMatcher)
    {
        _env = env;
        routeMatcherFactory = routeMatcher;
    }

    public Guid? GetPageGuid(string? route)
    {
        Guid guid;
        string? match;
        if (Routes is null)
            Initialize();

        match = MatchRoute(route);
        
        if(Routes!.TryGetValue(match, out guid))
            return guid;

        return null;
    }

    private string MatchRoute(string? route)
    {
        if (route is null)
            return "Home/Index";

        Regex regex = routeMatcherFactory
            .Create(route);

        foreach(var defined in ExistingRoutes)
        {
            if (regex.IsMatch(defined))
                return defined;
        }
        return "NotFound/Index";
    }
    public async Task<Guid?> GetPageGuidAsync(string? route)
    {
        var guid = GetPageGuid(route);
        if (guid is not null)
            return guid;
        if(_routes is null)
            await InitializeAsync();

        return GetPageGuid(route);
    }
    public IRouteRepository Initialize()
    {
        _routes = new();
        using (var r = new StreamReader(File.OpenRead(Path.Combine(_env.WebRootPath, "Pages", "ExistingPages.txt"))))
        {
            string? line = null;
            while ((line = r.ReadLine()) is not null)
            {
                if(line.Contains(delimiter))
                    AddOnsplit(line);
                Console.WriteLine($"\t {line}\t \t AddOnsplit");
            }
        }
        return this;
    }
    /// <summary>
    /// bad pattern, this is a helper, should be moved out as a extensionhelper
    /// </summary>
    /// <param name="input"></param>
    private void AddOnsplit(string input)
    {
        var split = input.Split(delimiter);
        _routes.Add(split[0], new Guid(split[1]));
    }
    private async Task ExcavatePages()
    {
        using (var r = new StringReader(Path.Combine(_env.ContentRootPath, "Pages", "ExistingPages")))
        {
            string? line = null;
            while((line = await r.ReadLineAsync()) is not null)
            {
                AddOnsplit(line);
            }
        }
    }
    public async Task<IRouteRepository> InitializeAsync()
    {
        await ExcavatePages();
        return this;
    }

    public IEnumerable<Guid> GetAvailableRoutes()
    {
        if(_routes.Values is null || _routes.Values.Count <= 0)
        {
            this.Initialize();
            return _routes.Values;
        }
        return _routes.Values;
    }

    public async Task<IEnumerable<Guid>> GetAvailableRoutesAsync()
    {
        if(_routes.Values is null || _routes.Values.Count <= 0)
        {
            await this.InitializeAsync();
            return _routes.Values;
        }
        return _routes.Values;
    }
}

