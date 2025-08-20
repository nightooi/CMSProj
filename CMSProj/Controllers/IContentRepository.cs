namespace CMSProj.Controllers
{
    /// <summary>
    /// something something IPrincipal, confirmed user, check credentials etc. etc.
    /// </summary>
    public interface IContentRepository : IPostActivator<IContentRepository>
    {
        IContentCache contentCache { get; }
        IEnumerable<string> GetPageContent(Guid guid);
        Task<IEnumerable<string>> GetPageConentAsync(Guid guid);
    }
}

