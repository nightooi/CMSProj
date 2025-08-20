namespace CMSProj.Controllers
{
    public interface IDynamicContent
    {
        public IRenderContent BuildEndClientContent<TRouteParams>(IEnumerable<string> content, Func<string, TRouteParams>? paramexcavation = null) where TRouteParams : class;
    }
}

