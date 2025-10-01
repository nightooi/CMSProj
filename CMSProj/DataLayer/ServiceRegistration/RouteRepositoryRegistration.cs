using CMSProj.DataLayer.PageServices.AdapterFactories;
using CMSProj.DataLayer.UrlServices;
using CMSProj.DataLayer.UrlServices.Factories;
using CMSProj.SubSystems.BackGroundServices.UrlUpdate;

namespace CMSProj.DataLayer.ServiceRegistration
{
    public static class RouteRepositoryRegistration
    {
        public static IServiceCollection AddRoutesServices(this IServiceCollection collection)
        {
            //IWorkResultOrchestrator<T> : IUpdateWorkResult<T>, IWorkResultManager<T>
            collection.AddSingleton<IUrlRepository, UrlRepository>();
            collection.AddSingleton<IActiveRouteManager, ActiveRoutesManager>();
            collection.AddSingleton<LogMessageFactory>();
            collection.AddSingleton<WorkerResultFactory<int>>();
            collection.AddSingleton<IWorkResultOrchestrator<WorkerResult<int>>, RouteManagerWorkerState>();
            collection.AddSingleton<IUpdateWorkResult<WorkerResult<int>>, RouteManagerWorkerState>();
            collection.AddSingleton<IWorkResultManager<WorkerResult<int>>, RouteManagerWorkerState>();
            collection.AddSingleton<IUpdateRouteManagerService, UpdateRouteManager>();
            collection.AddSingleton<IDatalayerFactory<UrlGuidAdapter, ContentDatabase.Model.PageSlug>, UrlGuidFactory>();
            collection.AddHostedService<RouteUpdateBackGroundService>();
            return collection;
        }
    }
}
