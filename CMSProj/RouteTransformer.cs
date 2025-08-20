using Microsoft.AspNetCore.Mvc.Routing;

using System.Text.RegularExpressions;

public class RouteTransformer : DynamicRouteValueTransformer
{
    private static readonly Regex _slugPattern =
        new(@"^([a-z0-9\-\/]){1,200}$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    private readonly IRouteRepository _routes;
    public RouteTransformer(IRouteRepository repo) => _routes = repo;


/// <summary>
/// #TODO: Need to expand the logic surrounding the url resolution to account for parameters and logical page directory expansion
///     ??
///     
/// </summary>
/// <param name="httpContext"></param>
/// <param name="values"></param>
/// <returns></returns>
    public override async ValueTask<RouteValueDictionary> TransformAsync
        (HttpContext httpContext,
        RouteValueDictionary values)
    {

        var route = (string?)values["slug"];

        if (route is null) 
            route = "Home/Index";

        if (!_slugPattern.IsMatch(route))
            route = "NotFound/Index";

        //
        // this pattern works for simple implementations but will create issues with page versioning.
        //  effectivly this throws out the complexity page versioning into the database, which.. well isn't optimal.
        //
        //      |
        //      V

        var guid = await _routes.GetPageGuidAsync(route);

        return new RouteValueDictionary()
        {
            ["controller"] = "DynamicPage",
            ["action"] = "RenderPage",
            ["pageGuid"] = guid.ToString()
        };
    }
}

