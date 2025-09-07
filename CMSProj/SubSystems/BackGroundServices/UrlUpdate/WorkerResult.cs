using CMSProj.DataLayer.UrlServices;

namespace CMSProj.SubSystems.BackGroundServices.UrlUpdate
{
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
}
