namespace CMSProj.SubSystems.Publishing
{
    //this type gets forwarded to the cshtml and injects the conent to the page.
    // NOICE

    public enum ComponentKeys { Header, Js, Scaffolding, Component, CompletePage}
    public interface IRenderContent
    {
       IDictionary<ComponentKeys, ICollection<string>> Content { get; }
       string InjectHeaders();
       string InjectBody();
       string InjectScripts();
    }

    public interface IRenderPluginContent
    {
       IEnumerable<string> InjectEndUserPlugins();
       IEnumerable<string> InjectPluginsHeaders();
       IEnumerable<string> InjectPluginsContent();
       IEnumerable<string> InjectHeaderPluginsJs();
    }

    public interface IRenderAdminContent
    {
       IEnumerable<string> InjectEndUserPlugins();
       IEnumerable<string> InjectPluginsHeaders();
       IEnumerable<string> InjectPluginsContent(bool isAuth);
       IEnumerable<string> InjectHeaderPluginsJs();
    }
}

