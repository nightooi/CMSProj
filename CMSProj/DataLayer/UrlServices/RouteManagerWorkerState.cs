using CMSProj.DataLayer.UrlServices.Factories;

namespace CMSProj.DataLayer.UrlServices
{
    public class RouteManagerWorkerState : WorkStateResult<int>
    {
        private LogMessageFactory _messageFactory;
        private List<DateTime> _stageStamps;
        private WorkerState _currentState;
        public override Task RunningTask { get; set; }
        protected override ILogger<IWorkResultOrchestrator<WorkerResult<int>>> Logger { get; set; }

        public RouteManagerWorkerState(WorkerResultFactory<int> resultFactory, LogMessageFactory messageFactory) : base(resultFactory)
        {
            _messageFactory = messageFactory;
            _resultFactory = resultFactory;
            _currentState = WorkerState.Initiated;
            _stageStamps = new();
        }

        public override void Log()
        {
            Logger.BeginScope($"{nameof(RouteManagerWorkerState)} Started Job:\t{WorkerResult.JobId}");
            foreach(var message in WorkerResult.LogMessage!)
            {
                Logger.Log(message.LogLevel, message.Message);
            }
        }

        public override WorkerResult<int> SnapShot()
        {
            //value type here so can just foroward
            return _resultFactory.Create(WorkerResult, () => WorkerResult.Result);
        }

        public override void UpdateWorkState<U>(U subService, WorkerState state, LogLevel logLevel)
        {
            switch (state)
            {
                case WorkerState.Failed:
                    UpdateResultOnFailure(nameof(subService), state, logLevel);
                    break;
                case WorkerState.Finnished:
                    UpdateOnFinish(nameof(subService), logLevel);
                    break;
                default:
                    UpdateOnContinuation(nameof(subService), state, logLevel);
                    break;
            }
        }
        void UpdateResultOnFailure(string subService, WorkerState state, LogLevel level)
        {
            Exception exc;
            var message = _messageFactory.Create();
            message.Message = $"\nWork {WorkerResult.JobId} Failed: {DateTime.UtcNow}." +
                $"\tStageFailure: {state}.\nException: {(RunningTask.Exception ?? new Exception()).Message}";
            message.LogLevel = level;
            WorkerResult.LogMessage!.Add(message);

            WorkerResult.Error = RunningTask.Exception;
            WorkerResult.Status += RunningTask.Exception is not null ? $"\nStage Failed With Exception: {RunningTask.Exception.Message}\t Time: {DateTime.UtcNow}\t" 
                : $"\nStage Failed But no exception was caught.";
            WorkerResult.Result = 0;
            WorkerResult.ProgressPercent += 25;
        }
        void UpdateOnContinuation(string subService, WorkerState state, LogLevel level)
        {
            var message = _messageFactory.Create();
            var continuationMessage = $"-- Finnished: {DateTime.UtcNow}\n {state}\t" +
                $" Duration{_stageStamps.Last() - _stageStamps[_stageStamps.Count-2]} Started: {DateTime.UtcNow}";

            message.Message = continuationMessage;
            message.LogLevel = level;
            WorkerResult.Status += continuationMessage;

            WorkerResult.LogMessage!.Add(message);
            WorkerResult.Result++;
        }
        void UpdateOnFinish(string subService, LogLevel level)
        {
            var message = _messageFactory.Create();
            var FinishMessage = $"Work: {WorkerResult.JobId} Finnshed Successfully: {DateTime.UtcNow}";
            WorkerResult.Status += $"-- Finnished: {DateTime.UtcNow}\n Op Ended with Total Time: {_stageStamps.Last() - _stageStamps.First()}";
            message.Message = FinishMessage;
            message.LogLevel = level;
            WorkerResult.LogMessage!.Add(message);
            WorkerResult.ProgressPercent += 25;
        }

        public override void UpdateCurrentTask(Task task)
        {
            RunningTask = task;
        }

        public override WorkerResult<int> InitializeWork()
        {
            return WorkerResult = _resultFactory.Create(Guid.NewGuid());
        }
        /// <summary>
        /// Ensure RunningTask prop is stopped.
        /// </summary>
        public override void Dispose()
        {
            RunningTask.Dispose();
        }
    }
}
