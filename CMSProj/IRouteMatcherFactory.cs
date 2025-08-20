using System.Text.RegularExpressions;


/// <summary>
/// Once support for more complex routes is implemented, this will have to grow in complexity
/// e.g specify domain
/// </summary>
public interface IRouteMatcherFactory
{
    Regex Create(string route);
}
public class RouteMatcherFactory : IRouteMatcherFactory
{
    public Regex Create(string route)
    {
        return new Regex(@$"(?<route>{route})", 
            RegexOptions.IgnoreCase |
            RegexOptions.Singleline |
            RegexOptions.IgnorePatternWhitespace |
            RegexOptions.NonBacktracking);
    }
}