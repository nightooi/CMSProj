namespace CMSProj.Controllers
{
    /// <summary>
    /// this type will only be used by the background worker constructing the page, unless there will be personal pages for the visitors
    /// </summary>
    /// <typeparam name="TOperation"></typeparam>
    public interface ICMSResult<TOperation> where TOperation : class
    {
        DBOpRes DBResult { get; }
        TOperation Operation { get; }
        public string DefaultContent { get; }
    }
}

