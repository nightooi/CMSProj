namespace CMSProj.Controllers
{
    public class RenderContent : IRenderContent
    {
        public IEnumerable<string> Content { get; set; }

        public IEnumerable<string> InjectBody()
        {
            return Content;
        }

        public IEnumerable<string> InjectCss()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> InjectHeaderPluginsJs(bool isAuth)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> InjectHeaders()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> InjectPluginsContent(bool isAuth)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> InjectPluginsHeaders(bool isAuth)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> InjectScripts()
        {
            throw new NotImplementedException();
        }
    }
}

