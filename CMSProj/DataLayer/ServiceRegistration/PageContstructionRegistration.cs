﻿using CMSProj.Controllers;
using CMSProj.DataLayer.PageServices;
using CMSProj.DataLayer.PageServices.AdapterFactories;
using CMSProj.DataLayer.PageServices.Components;
using CMSProj.DataLayer.PageServices.Repo;
using CMSProj.SubSystems.Publishing;
namespace CMSProj.DataLayer.ServiceRegistration
{
    public static class PageContstructionRegistration
    {
        public static IServiceCollection AddPageServices(this IServiceCollection collection)
        {
            collection.AddScoped<IAssetLocator, PrepAssetsService>();
            collection.AddScoped<IDatalayerFactory<ContentComponent, ContentDatabase.Model.AuthoredComponent>, ContentComponentFactory>();
            collection.AddScoped<IDatalayerFactory<PageAdapter, ContentDatabase.Model.Page>, PageAdapterFactory>();
            collection.AddScoped<IDatalayerFactory<ScaffoldAdapter, ContentDatabase.Model.PageTemplate>, ScaffoldAdapterFactory>();
            collection.AddScoped<IDatalayerFactory<AssetAdapter, ContentDatabase.Model.Assets>, AssetFactory>();
            collection.AddScoped<IPageManager, PublicationManger>();
            collection.AddScoped<IPublishedPageRepository, PublishedPageRepo>();
            collection.AddScoped<IContentExcavationService<ScaffoldAdapter>, ScaffoldingManager>();
            collection.AddSingleton<IMenuCreator, MenuCreator>();
            collection.AddScoped<IMenuManger, MenuManager>();
            collection.AddScoped<IComponentsRepo, ComponentRepo>();
            collection.AddTransient<IContentBuilder, ContentBuilder>();
            collection.AddTransient<IRenderContent, RenderContent>();
            return collection;
        }
    }
}
