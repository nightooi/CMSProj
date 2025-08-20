namespace CMSProj.Controllers
{
    /// <summary>
    /// Admin stuff, this should sit on the RestClient.
    /// </summary>
    public interface IEndPointSomething
    {
        ICMSResult<TOperation> EditPageContent<TOperation>(Guid page) where TOperation : class;
        ICMSResult<TOperation> AddPageContent<TOperation>(Guid page) where TOperation : class;
        ICMSResult<TOperation> AddPage<TOperation>(TOperation pageData) where TOperation : class;
    }
}

