namespace CMSProj.Controllers
{
    //this type gets forwarded to the cshtml and injects the conent to the page.
    // NOICE
    public interface IRenderContent
    {
       IEnumerable<string> Content { get; set; }
       IEnumerable<string> InjectHeaders();
       IEnumerable<string> InjectCss();
       IEnumerable<string> InjectBody();
       IEnumerable<string> InjectScripts();
       IEnumerable<string> InjectPluginsHeaders(bool isAuth);
       IEnumerable<string> InjectPluginsContent(bool isAuth);
       IEnumerable<string> InjectHeaderPluginsJs(bool isAuth);
    }
}

