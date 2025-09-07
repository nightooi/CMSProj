using CMSProj.Controllers;
namespace CMSProj.DataLayer
{
    public static class PageContstructionRegistration
    {
        public static IServiceCollection AddPageServices(this IServiceCollection collection)
        {
            collection.AddScoped<IAssetLocator, PrepAssetsService>();
            collection.AddScoped<IDatalayerFactory<CMSProj.DataLayer.ContentComponent, ContentDatabase.Model.AuthoredComponent>, ContentComponentFactory>();
            collection.AddScoped<IDatalayerFactory<CMSProj.DataLayer.PageAdapter, ContentDatabase.Model.Page>, PageAdapterFactory>();
            collection.AddScoped<IDatalayerFactory<CMSProj.DataLayer.ScaffoldAdapter, ContentDatabase.Model.PageTemplate>, ScaffoldAdapterFactory>();
            collection.AddScoped<IDatalayerFactory<CMSProj.DataLayer.AssetAdapter, ContentDatabase.Model.Assets>, AssetFactory>();
            collection.AddScoped<IPageRepository, PageRepository>();
            collection.AddScoped<IPublishedPageRetrieval, PageRetrieval>();
            collection.AddScoped<IContentExcavationService<ScaffoldAdapter>, ScaffoldingRetrievalService>();
            collection.AddTransient<IContentBuilder, ContentBuilder>();
            collection.AddTransient<IRenderContent, RenderContent>();
            return collection;
        }
    }
}
