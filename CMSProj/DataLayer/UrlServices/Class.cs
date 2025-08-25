using CMSProj.DataLayer;

using ContentDatabase;

using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

using NuGet.Packaging.Signing;

using System.ComponentModel;
using System.Linq.Expressions;

namespace CMSProj.DataLayer.UrlServices
{
    public interface IUrlRetrievalService
    {
        public ICollection<UrlGuidAdapter> GetUrls();
        public Task<ICollection<UrlGuidAdapter>> GetUrlsAsync(CancellationToken token);
    }

    public static class ContentDBExtensions
    {
        public static ICollection<UrlGuidAdapter> SelectPublishedPages(this DbSet<ContentDatabase.Model.Page> pages,
            Func<PageProxy, UrlGuidAdapter> transformer)
        {
            var res = pages
                 .Where(x => x.Published <= DateTime.UtcNow)
                 .Select(x => new { guid = x.Id, url = x.Slug })
                 .ToList();
            return res.Select(x => transformer(new PageProxy(x.guid, x.url))).ToList();
        }
        public static ICollection<UrlGuidAdapter> SelectPublishedPageWithPublishedComponents(this DbSet<ContentDatabase.Model.Page> pages,
           Func<PageProxy, UrlGuidAdapter> transformer)
        {
            var res = pages
                 .Include(x => x.PageComponenets)
                 .Where(x => x.Published <= DateTime.UtcNow && x.PageComponenets.All(x => x.Published <= DateTime.UtcNow))
                 .Select(x => new { guid = x.Id, url = x.Slug })
                 .ToList();
            return res.Select(x => transformer(new PageProxy(x.guid, x.url))).ToList();
        }
        public static IQueryable<TResult> SelectPublishedPageWithPublishedComponents<TResult>(this DbSet<ContentDatabase.Model.Page> pages,
            Expression<Func<ContentDatabase.Model.Page, TResult>> selector)
        {
            return pages
                 .Include(x => x.PageComponenets)
                 .Where(x => x.Published <= DateTime.UtcNow && x.PageComponenets.All(x => x.Published <= DateTime.UtcNow))
                 .Select(selector);
        }
        public static async Task<ICollection<UrlGuidAdapter>> SelectPublishedPageWithPublishedComponentsAsync(this DbSet<ContentDatabase.Model.Page> pages,
            Func<PageProxy, UrlGuidAdapter> transformer)
        {
            var res = await pages
                .SelectPublishedPageWithPublishedComponents(x => new { id = x.Id, slug = x.Slug })
                .ToListAsync();

            return res.Select(x => transformer(new PageProxy(x.id, x.slug)))
                .ToList();
        }

    }

    public class UrlRetrival : IUrlRetrievalService
    {
        ContentContext _ctx;
        IDatalayerFactory<UrlGuidAdapter, ContentDatabase.Model.Page> _adapterFactory;

        public UrlRetrival(ContentContext ctx, IDatalayerFactory<UrlGuidAdapter, ContentDatabase.Model.Page> adapterFactory)
        {
            _ctx = ctx;
            _adapterFactory = adapterFactory;
        }
        public ICollection<UrlGuidAdapter> GetUrls()
        {
            return _ctx.Pages.SelectPublishedPageWithPublishedComponents(_adapterFactory.Create);
        }

        public async Task<ICollection<UrlGuidAdapter>> GetUrlsAsync(CancellationToken token)
        {
            return await _ctx.Pages.SelectPublishedPageWithPublishedComponentsAsync(_adapterFactory.Create);
        }
    }
    public class LogMessage
    {
        public string Message { get; set; }
        public LogLevel LogLevel { get; set; }
    }
    public class WorkerResult<T>
    {
        public Guid JobId { get; init; }           
        public string Status { get; set; }        
        public int? ProgressPercent { get; set; }
        public List<LogMessage>? LogMessage { get; set; }     
        public Exception? Error { get; set; }  
        public T? Result { get; set; }        
        public LogLevel LogLevel { get; set; }  
        public DateTime Timestamp { get; init; } = DateTime.UtcNow;
        public string WorkType => nameof(T);

        public WorkerResult()
        {

        }
        /// <summary>
        /// dont pass resultCopy reference if T is a object.
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="resultCopy"></param>
        public WorkerResult(WorkerResult<T> worker, Func<T> resultCopy)
        {
            JobId = new Guid(worker.JobId.ToString());
            Status = worker.Status;
            ProgressPercent = ProgressPercent;
            Error = new Exception(null, worker.Error);
            LogLevel = worker.LogLevel;
            Result = resultCopy();
            Timestamp = worker.Timestamp;
            LogMessage = new List<LogMessage>();
        }
    }
    public class LogMessageFactory
    {
        public LogMessage Create()
        {
            return new LogMessage();
        }
        public LogMessage Create(LogMessage message)
        {
            return new LogMessage()
            {
                Message = message.Message,
                LogLevel = message.LogLevel
            };
        }
    }
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
    public enum WorkerState { Initiated, Retrieving, Sorting, MergingManager, Finnished,Failed }
    public interface IWorkResultOrchestrator<T>
    {
        public Task RunningTask { get; set; }
        public void UpdateWorkState<U>(U subService, WorkerState state, LogLevel logLevel);
        public void Log();
        public T SnapShot();
    }

    public abstract class WorkStateResult<T> : IWorkResultOrchestrator<WorkerResult<T>>
    {
        protected WorkerResultFactory<T> _resultFactory;
        protected WorkerResult<T> _result;
        protected abstract ILogger<IWorkResultOrchestrator<WorkerResult<T>>> Logger { get; set; }
        public abstract Task RunningTask { get; set; }

        public abstract void Log();
        public abstract WorkerResult<T> SnapShot();
        public abstract void UpdateWorkState<U>(U subService, WorkerState state, LogLevel logLevel);
        public WorkStateResult(WorkerResultFactory<T> factory)
        {
            _resultFactory = factory;
            _result = _resultFactory.Create(Guid.NewGuid());
        }
    }
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
            Logger.BeginScope($"{nameof(RouteManagerWorkerState)} Started Job:\t{_result.JobId}");
            foreach(var message in _result.LogMessage!)
            {
                Logger.Log(message.LogLevel, message.Message);
            }
        }

        public override WorkerResult<int> SnapShot()
        {
            //value type here so can just foroward
            return _resultFactory.Create(_result, () => _result.Result);
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
            message.Message = $"\nWork {_result.JobId} Failed: {DateTime.UtcNow}." +
                $"\tStageFailure: {state}.\nException: {(RunningTask.Exception ?? new Exception()).Message}";
            message.LogLevel = level;
            _result.LogMessage!.Add(message);

            _result.Error = RunningTask.Exception;
            _result.Status += RunningTask.Exception is not null ? $"\nStage Failed With Exception: {RunningTask.Exception.Message}\t Time: {DateTime.UtcNow}\t" 
                : $"\nStage Failed But no exception was caught.";
            _result.Result = 0;
            _result.ProgressPercent += 25;
        }
        void UpdateOnContinuation(string subService, WorkerState state, LogLevel level)
        {
            var message = _messageFactory.Create();
            var continuationMessage = $"-- Finnished: {DateTime.UtcNow}\n {state}\t" +
                $" Duration{_stageStamps.Last() - _stageStamps[_stageStamps.Count-2]} Started: {DateTime.UtcNow}";

            message.Message = continuationMessage;
            message.LogLevel = level;
            _result.Status += continuationMessage;

            _result.LogMessage!.Add(message);
            _result.Result++;
        }
        void UpdateOnFinish(string subService, LogLevel level)
        {
            var message = _messageFactory.Create();
            var FinishMessage = $"Work: {_result.JobId} Finnshed Successfully: {DateTime.UtcNow}";
            _result.Status += $"-- Finnished: {DateTime.UtcNow}\n Op Ended with Total Time: {_stageStamps.Last() - _stageStamps.First()}";
            message.Message = FinishMessage;
            message.LogLevel = level;
            _result.LogMessage!.Add(message);
            _result.ProgressPercent += 25;
        }
    }
    public interface IUpdateRouteManagerService
    {
        public void ScheduleUpdate(DateTime time);
        public Task UpdateRoutes(CancellationToken token);
    }
    public interface IRouteUpdateOrchestrator
    {
        public Guid WorkGuid { get; }
        public Task RunningTask { get; }
        public CancellationToken CancellationToken { get; }
        public WorkerState CurrentState { get; }
        public IReadOnlyCollection<TimeOnly> StageStamps { get; }
        public TimeSpan UpdateInterval { get; }
    }
    public class UpdateRouteManager : IUpdateRouteManagerService, IDisposable
    {
        public List<DateTime> _updateSchedule = new();

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

        private IRouteRepository RouteRepository { get; set; }
        public Task RunningTask => UpdateRoutes(_cancellationToken);

        private IWorkResultOrchestrator<WorkerResult<int>> _resultOrchestrator;

        public UpdateRouteManager(
            IRouteRepository routeRepository, 
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

        public void ScheduleUpdate(DateTime time)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    public class UrlGuidAdapter
    {
        public Guid Guid { get; set; }
        public string PageUrl { get; set; }
    }
    //pageproxy
    public class PageProxy : ContentDatabase.Model.Page
    {
        public new Guid Id { get; set; }
        public PageProxy(Guid id, string slug)
        {
            Id = id;
            Slug = slug;
        }
    }
    public class UrlGuidFactory : IDatalayerFactory<CMSProj.DataLayer.UrlServices.UrlGuidAdapter, PageProxy> 
    {
        public UrlGuidAdapter Create(PageProxy page)
        {
            return new UrlGuidAdapter()
            {
                Guid = page.Id,
                PageUrl = page.Slug
            };
        }
    }
}
