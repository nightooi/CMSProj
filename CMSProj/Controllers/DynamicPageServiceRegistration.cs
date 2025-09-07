namespace CMSProj.Controllers
{
    public static class DynamicPageServiceRegistration
    {
        public static IServiceCollection RegisterDynmicServices(this IServiceCollection services)
        {
            services.AddTransient<IRenderContent, RenderContent>();
            services.AddSingleton<RouteTransformer>();
            return services;
        }
    }
}
