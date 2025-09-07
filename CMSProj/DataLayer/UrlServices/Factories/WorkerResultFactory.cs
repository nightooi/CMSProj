using CMSProj.SubSystems.BackGroundServices.UrlUpdate;

namespace CMSProj.DataLayer.UrlServices.Factories
{
    public class WorkerResultFactory<T>
    {
        private LogMessageFactory LogMessageFactory { get; set; }
        public WorkerResult<T> Create(Guid guid)
        {
            return new WorkerResult<T>()
            {
                JobId = guid,
                Status = $"Worker {nameof(IUpdateRouteManagerService)} Initiated: {WorkerState.Initiated} \t Started: {DateTime.UtcNow}",
                LogMessage = new List<LogMessage>(),
                ProgressPercent = 0,
                LogLevel = LogLevel.Information,
            };
        }
        public WorkerResult<T> Create(WorkerResult<T> result, Func<T> deepCopy)
        {
            return new WorkerResult<T>(result, deepCopy);
        }
    }
}
