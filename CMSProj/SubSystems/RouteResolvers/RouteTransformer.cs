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
            route = "Home";

        if (!_slugPattern.IsMatch(route))
            route = "NotFound";

        //
        // this pattern works for simple implementations but will create issues with page versioning.
        //  effectivly this throws out the complexity page versioning into the database, which.. well isn't optimal.
        //
        //      |
        //      V
        // Guess if i didnt have to solve this within the dbmodel too.....
        //      and now there are issues with lifetimes across the service chain because of this
        //      (:
        //
        // Okay, so the lifetimes issues stemmed from the fact that the routerepo is serving both as a hotcache 
        // and a retrieval service.. I don't see a quick way to solve this other than create another layer of inderaction
        // which would hold the cached routes since last update. Either way the solution was to remove the context as a 
        // parameter to it and instantiate a scope where the db calls inside the Routerepo was done, since there's no defering
        // and the call to the db is evaluated immediately and the entities immediately deep copied it works.
        // 
        var routeL = httpContext.RequestServices.GetRequiredService<IRouteRepository>();
        var guid = await routeL.GetPageGuidAsync(route, httpContext.RequestAborted);

        return new RouteValueDictionary()
        {
            ["controller"] = "DynamicPage",
            ["action"] = "RenderPage",
            ["pageGuid"] = guid.ToString()
        };
    }
}

