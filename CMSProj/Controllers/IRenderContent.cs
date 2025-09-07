using CMSProj.DataLayer;

using System.Text;

namespace CMSProj.Controllers
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

    public interface IContentBuilder
    {
        IRenderContent BuildContent(PageAdapter adapter);
    }

    public class ContentBuilder : IContentBuilder
    {
        IRenderContent _content;
        public ContentBuilder(IRenderContent rendercontent)
        {
            _content = rendercontent;
        }
        public IRenderContent BuildContent(PageAdapter adapter)
        {
            _content.Content.Add(ComponentKeys.Scaffolding, []);
            _content.Content[ComponentKeys.Scaffolding] = adapter.Scaffolding.ComponentHtml.ToList();
            _content.Content.Add(ComponentKeys.Header, [adapter.HeaderContents ?? ""]);
            _content.Content.Add(ComponentKeys.Js, [adapter.JsContents ?? ""]);
            _content.Content.Add(ComponentKeys.Component, []);
            _content.Content[ComponentKeys.Component] = adapter.PageContent.MergeContentWithComponent().ToList();
            _content.Content.Add(ComponentKeys.CompletePage, []);
            _content.Content[ComponentKeys.CompletePage] = adapter.Scaffolding.MergeComponentWithScaffold(adapter.PageContent).ToList();
            return _content;
        }
    }

    public static class HtmlMergingExtensions
    {
        public static IEnumerable<string> MergeComponentWithScaffold(this ScaffoldAdapter adapter, IReadOnlyCollection<ContentComponent> component)
        {
            var join = adapter.RenderChildOffsets.Join(component,
                    x => x.ComponentGuid,
                    y => y.ScaffoldAdapterId,
                    (x, y) =>
                    new { offset = x.RenderOffset, content = y.MergeComponent((x, y) => x)});
           // int i = 0;
           // foreach(var scaffold in adapter.ComponentHtml)
           // {
           //     StringBuilder builder = new(scaffold);
           //     yield return builder
           //         .Insert(
           //         join.ElementAt(i).offset,
           //         join.ElementAt(i).content)
           //         .ToString();
           //     i++;
           // }
            var que = new Queue<string>(adapter.ComponentHtml.Select(x => x));
            foreach(var element in join)
            {
                StringBuilder builder = new(que.Dequeue());
                yield return builder.Insert(element.offset, element.content).ToString();
            }
        }
        public static IEnumerable<string> MergeContentWithComponent(this IReadOnlyCollection<ContentComponent> components)
        {
            return components.MergeContentWithComponent((x, y) => x);
        }

        public static IEnumerable<TResult> MergeContentWithComponent<TResult>(this IReadOnlyCollection<ContentComponent> components, Func<string, ContentComponent, TResult> func)
        {
            foreach(var component in components)
            {
                yield return MergeComponent(component, func);
            }
 
        }
        public static TResult MergeComponent<TResult>(this ContentComponent component, Func<string, ContentComponent, TResult> func)
        {
           var join = component.Content.Content.Join(component.Html.Content,
                x => x.Id,
                y => y.ContentId,
                (x, y) =>
                new { offset = y.Offset, content = x.Content });

            StringBuilder val = new StringBuilder(component.Html.Markup);
            int i = 0;
            foreach(var item in join)
            {
                val.Insert(item.offset + (i > 0 ? join.ElementAt(i - 1).content.Length : 0), item.content);
                i++; 
            }
            return func(val.ToString(), component);
        }
    }
}

