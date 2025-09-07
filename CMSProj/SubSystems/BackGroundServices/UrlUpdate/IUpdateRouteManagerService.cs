namespace CMSProj.SubSystems.BackGroundServices.UrlUpdate
{
    public interface IUpdateRouteManagerService
    {
        public Task UpdateRoutes(CancellationToken token);
    }
}
