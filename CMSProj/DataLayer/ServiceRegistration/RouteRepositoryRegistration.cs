using CMSProj.DataLayer.UrlServices;

namespace CMSProj.DataLayer.ServiceRegistration
{
    public static class RouteRepositoryRegistration
    {
        public static IServiceCollection AddRoutesServices(this IServiceCollection collection)
        {
            //IWorkResultOrchestrator<T> : IUpdateWorkResult<T>, IWorkResultManager<T>
            collection.AddSingleton<IUrlRetrievalService, UrlRetrival>();
            collection.AddSingleton<IRouteRepository, DbRouteRepository>();
            collection.AddSingleton<LogMessageFactory>();
            collection.AddSingleton<WorkerResultFactory<WorkerResult<int>>>();
            collection.AddSingleton<IWorkResultOrchestrator<WorkerResult<int>>, RouteManagerWorkerState>();
            collection.AddSingleton<IUpdateWorkResult<WorkerResult<int>>, RouteManagerWorkerState>();
            collection.AddSingleton<IWorkResultManager<WorkerResult<int>>, RouteManagerWorkerState>();
            collection.AddSingleton<IUpdateRouteManagerService, UpdateRouteManager>();
            collection.AddHostedService<RouteUpdateBackGroundService>();
            return collection;
        }
    }
}
