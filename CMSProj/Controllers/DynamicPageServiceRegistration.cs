namespace CMSProj.Controllers
{
    public static class DynamicPageServiceRegistration
    {
        public static IServiceCollection RegisterDynmicServices(this IServiceCollection services)
        {
            //services.AddSingleton<IContentCache, ContentCache>();
            services.AddSingleton<IContentRepository, FileSystemContentRepo>();
            services.AddTransient<IRenderContent, RenderContent>();
            services.AddScoped<IDynamicContent, DynamicContent>();
            services.AddSingleton<RouteTransformer>();
            return services;
        }
    }
}
