public static class RegisterRouteRepo
{
    public static IServiceCollection AddExistingRoutesHandler(this IServiceCollection collection)
    {
        collection.AddSingleton<IRouteMatcherFactory, RouteMatcherFactory>();
        return collection;
    }
}

