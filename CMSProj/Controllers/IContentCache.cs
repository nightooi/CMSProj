namespace CMSProj.Controllers
{
    public interface IContentCache : ICache<Guid, IEnumerable<string>>, IPostActivator<IContentCache>
    {
        public IEnumerable<string> GetFullPage(Guid key);
    }
}

