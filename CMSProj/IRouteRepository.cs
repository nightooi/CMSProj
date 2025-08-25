public interface IRouteRepository : IPostActivator<IRouteRepository>
{
    public Task<Guid?> GetPageGuidAsync(string? route);
    public Guid? GetPageGuid(string? route);

    //There should be a way to redo this with options pattern
    // likely should be too
    public void GetAvailableRoutes();
    Task GetAvailableRoutesAsync(CancellationToken token);
}

