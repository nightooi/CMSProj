namespace CMSProj.DataLayer.UrlServices
{
    /// <summary>
    /// Started rework of the workupdate interface.
    /// 
    /// I noticed that the lifetimes across the dependcy chains didnt line up,
    ///     the routerepo(singleton) depended on workresultorchestrator which theoretichally should be:
    ///     scoped or even a transient instance as it's tied to the current updatecycle.
    ///     
    ///     solution one is make the orchestrator a singleton and extract the Management of the result object(creation/disposal)
    ///     into one interface, and the updates that happen down the chain into another and have the objects down the chain
    ///     depend on only the update interface.
    ///     Resolution of the update interface object would be done by a factory directly depending on the scope created in the worker.
    ///         -> doesnt work. :D service providers cant cross scope apparently
    ///             so we go the singleton route
    ///     
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IUpdateWorkResult<T>
    {
        public Task RunningTask { get; set; }
        public void UpdateWorkState<U>(U subService, WorkerState state, LogLevel logLevel);
        public void UpdateCurrentTask(Task task);
        public T SnapShot();
    }
}
