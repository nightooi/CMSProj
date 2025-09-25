using Microsoft.CodeAnalysis.CSharp.Syntax;

using NuGet.Packaging.Signing;

using System.ComponentModel;

namespace CMSProj.SubSystems.BackGroundServices.UrlUpdate
{
    public class RouteUpdateBackGroundService : BackgroundService
    {

        private IServiceScopeFactory _scopeFactory;

        public RouteUpdateBackGroundService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var timer = new PeriodicTimer(TimeSpan.FromMinutes(1));
            while(await timer.WaitForNextTickAsync(stoppingToken))
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var updateRoutes = scope.ServiceProvider.GetRequiredService<IUpdateRouteManagerService>();
                    var resultManager = scope.ServiceProvider.GetRequiredService<IWorkResultOrchestrator<WorkerResult<int>>>();
                    //
                    //this is a statefull pattern... it kindofworks...
                    //but honestly the initialize work method should just be called within updateworkstate in order to keep the interface stateless
                    //
                    resultManager.InitializeWork();
                    resultManager.UpdateWorkState(this, WorkerState.Initiated, LogLevel.Information | LogLevel.Debug);
                    resultManager.RunningTask = updateRoutes.UpdateRoutes(stoppingToken);
                    await resultManager.RunningTask;
                    if (resultManager.RunningTask.IsFaulted)
                    {
                        resultManager.UpdateWorkState(this, WorkerState.Failed, LogLevel.Error | LogLevel.Debug);
                    }
                    else if (resultManager.RunningTask.IsCompletedSuccessfully)
                    {
                        resultManager.UpdateWorkState(this, WorkerState.Finnished, LogLevel.Information | LogLevel.Debug);
                    }
                    resultManager.Log();
                }
            }
        }
    }
}
