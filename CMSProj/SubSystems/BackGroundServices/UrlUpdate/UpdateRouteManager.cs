namespace CMSProj.SubSystems.BackGroundServices.UrlUpdate
{
    public class UpdateRouteManager : IUpdateRouteManagerService, IDisposable
    {

        readonly TimeSpan _span = TimeSpan.FromMinutes(5);
        public TimeSpan UpdateInterval => _span;
        public ILogger<UpdateRouteManager> _logger { get; set; }

        ICollection<TimeOnly> _stageStamps;
        public IReadOnlyCollection<TimeOnly> StageStamps =>(IReadOnlyCollection<TimeOnly>)_stageStamps;

        WorkerState _currentState;
        public WorkerState CurrentState => _currentState;

        CancellationToken _cancellationToken;
        public CancellationToken CancellationToken => _cancellationToken;
        Guid _workGuid;
        public Guid WorkGuid => _workGuid;

        private IActiveRouteManager RouteRepository { get; set; }
        public Task RunningTask => UpdateRoutes(_cancellationToken);

        private IWorkResultOrchestrator<WorkerResult<int>> _resultOrchestrator;

        public UpdateRouteManager(
            IActiveRouteManager routeRepository, 
            ILogger<UpdateRouteManager> logger, 
            IWorkResultOrchestrator<WorkerResult<int>> workResultOrchestrator)
        {
            _resultOrchestrator = workResultOrchestrator;
            _currentState = WorkerState.Initiated;
            _workGuid = Guid.NewGuid();
            _stageStamps = new List<TimeOnly>(4);
            RouteRepository = routeRepository;
        }
        //Immediate Call 
        public async Task UpdateRoutes(CancellationToken token)
        {
            _resultOrchestrator.UpdateWorkState(this, WorkerState.Retrieving, LogLevel.Information);
            await RouteRepository.GetAvailableRoutesAsync(token);
        }

        public void Dispose()
        {
            if (RunningTask.IsCompleted)
            {
                RunningTask.Dispose();
            }
        }
    }
}
