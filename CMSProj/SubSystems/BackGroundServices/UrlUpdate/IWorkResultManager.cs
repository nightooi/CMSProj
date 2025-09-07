namespace CMSProj.SubSystems.BackGroundServices.UrlUpdate
{
    public interface IWorkResultManager<T> : IDisposable
    {
        public T? WorkerResult { get; set; }
        public void Log();
        public T InitializeWork();
    }
}
