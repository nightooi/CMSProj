using CMSProj.DataLayer.UrlServices.Factories;

namespace CMSProj.SubSystems.BackGroundServices.UrlUpdate
{
    //Singleton, Its state is managed by the BackgroundService, appears stateless down the dependency tree
    public abstract class WorkStateResult<T> : IWorkResultOrchestrator<WorkerResult<T>>
    {
        protected WorkerResultFactory<T> _resultFactory;
        protected abstract ILogger<IWorkResultOrchestrator<WorkerResult<T>>> Logger { get; set; }
        public abstract Task RunningTask { get; set; }

        public WorkerResult<T>? WorkerResult { get; set; }
        public abstract void Log();
        public abstract WorkerResult<T> SnapShot();
        public abstract void UpdateWorkState<U>(U subService, WorkerState state, LogLevel logLevel);
        public abstract void UpdateCurrentTask(Task task);
        public abstract WorkerResult<T> InitializeWork();
        public abstract void Dispose();

        public WorkStateResult(WorkerResultFactory<T> factory)
        {
            _resultFactory = factory;
            WorkerResult = _resultFactory.Create(Guid.NewGuid());
        }
    }
}
