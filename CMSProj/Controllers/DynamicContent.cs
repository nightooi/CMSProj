namespace CMSProj.Controllers
{
    public class DynamicContent(IRenderContent contentPage) : IDynamicContent
    {
        IRenderContent _contentPage = contentPage;
        public IRenderContent BuildEndClientContent<TRouteParams>(IEnumerable<string> content, Func<string, TRouteParams>? paramexcavation = null) 
            where TRouteParams : class
        {
            _contentPage.Content = content;
            return _contentPage;
        }
    }
}

