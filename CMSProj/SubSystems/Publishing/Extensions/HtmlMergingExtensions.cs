using CMSProj.DataLayer.PageServices.Components;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using System.Text;

namespace CMSProj.SubSystems.Publishing.Extensions
{
    public static class HtmlMergingExtensions
    {
        public static IEnumerable<string> MergeComponentWithScaffold(this ScaffoldAdapter adapter, IReadOnlyCollection<ContentComponent> component)
        {
            var join = adapter.ComponentHtml.Join(component,
                    x => x.offset.ComponentGuid,
                    y => y.ScaffoldAdapterId,
                    (x, y) =>
                    new { offset = x.offset.RenderOffset, content = y.SimplifiedContent((x, y) => x)});
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
            var g = new Guid().ToString().Length + " data-cmsrootcompguid=".Length +2;
            foreach(var element in join.ToList())
            {
                var doc =new  HtmlAgilityPack.HtmlDocument();
                var elem = adapter.ComponentHtml.Dequeue();
                doc.LoadHtml(elem.html);

                var child = new HtmlAgilityPack.HtmlDocument();
                child.LoadHtml(element.content);

                doc.DocumentNode.SelectSingleNode("//*").AppendChild(child.DocumentNode);
                var node = doc.DocumentNode.SelectSingleNode("//*");

                node.SetAttributeValue("data-cmsrootcompguid", elem.Guid.ToString());

                yield return doc.DocumentNode.OuterHtml;
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
                yield return component.SimplifiedContent(func);
            }
 
        }
        public static TResult MergeComponent<TResult>(this ContentComponent component, Func<string, ContentComponent, TResult> func)
        {
           var join = component.Content.Content.Join(component.Html.Content,
                x => x.Id,
                y => y.ContentId,
                (x, y) =>
                new { offset = y.Offset, content = x.Content })
                .OrderBy(x=> x.offset)
                .ToArray();

            StringBuilder val = new StringBuilder(component.HtmlMarkup);
            int i = 0;
            foreach(var item in join)
            {
                val.Insert(item.offset, item.content);
                i++; 
            }
            return func(val.ToString(), component);
        }
        private static TResult SimplifiedContent<TResult>(this ContentComponent component, Func<string, ContentComponent, TResult> func)
        {
            return func(component.HtmlMarkup, component);
        }
    }
}

