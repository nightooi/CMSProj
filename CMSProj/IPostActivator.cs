public interface IPostActivator<T> where T : class
{
    T Initialize();
    Task<T> InitializeAsync(CancellationToken token);
}

