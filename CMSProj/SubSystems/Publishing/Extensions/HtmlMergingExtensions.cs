using CMSProj.DataLayer.PageServices.Components;

using System.Text;

namespace CMSProj.SubSystems.Publishing.Extensions
{
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
            var g = new Guid().ToString().Length + " data-cmsrootcompguid=".Length +2;
            foreach(var element in join)
            {
                var doc =new  HtmlAgilityPack.HtmlDocument();
                var elem = adapter.ComponentHtml.Dequeue();

                doc.LoadHtml(elem.html);
                var node = doc.DocumentNode.SelectSingleNode("/*");
                node.SetAttributeValue("data-cmsrootcompguid", elem.Guid.ToString());
                var html = doc.DocumentNode.OuterHtml;

                StringBuilder builder = new(html);
                yield return builder.Insert(element.offset+g, element.content).ToString();
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
                yield return component.MergeComponent(func);
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

            StringBuilder val = new StringBuilder(component.Html.Markup);
            int i = 0;
            foreach(var item in join)
            {
                val.Insert(item.offset, item.content);
                i++; 
            }
            return func(val.ToString(), component);
        }
    }
}

