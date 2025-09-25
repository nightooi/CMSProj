namespace CMSProj.SubSystems.Publishing.Registration
{
    public static class DynamicPageServiceRegistration
    {
        public static IServiceCollection AddDynmicRouteServices(this IServiceCollection services)
        {
            services.AddTransient<IRenderContent, RenderContent>();
            services.AddSingleton<RouteTransformer>();
            return services;
        }
    }
}
