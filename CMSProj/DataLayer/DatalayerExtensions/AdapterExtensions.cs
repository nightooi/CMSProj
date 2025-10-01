using CMSProj.DataLayer.PageServices.Components;

using ContentDatabase.Model;

using HtmlAgilityPack;

using System.Text;
namespace CMSProj.DataLayer.DatalayerExtensions
{
    public static class AdapterExtensions
    {
        public static ICollection<TResult> ExtractAssetsExt<TIn, TResult>(this ICollection<TIn> assets, Func<TIn, TResult> factory)
        {
            var compiled = new List<TResult>();
            foreach(var asset in assets)
            {
                compiled.Add(factory(asset));
            }
            return compiled;
        }
        public static Peripherals ExtractPeripheralsExt<T>(this ICollection<T> values) where T : IComponentPeripheral
        {
            var headers = (Peripherals)new PageAdapter(new Guid(), DateTime.UtcNow);
            StringBuilder css = new();
            StringBuilder outsideHeader = new();
            StringBuilder headerJs = new();
            StringBuilder js = new ();
            foreach(var value in values)
            {
                outsideHeader.Append(Interpolateheaders(value, value => value.OtherHeaders));
                css.Append(Interpolateheaders(value, value => value.CssHeaderTags));
                headerJs.Append(Interpolateheaders(value, value => value.JsHeaderTags));
                js.Append(Interpolateheaders(value, value => value.JsBodyTags));
            }
            outsideHeader.Append(css);
            outsideHeader.Append(headerJs);
            headers.HeaderContents = outsideHeader.ToString();
            headers.JsContents = js.ToString();
            return headers;
        }
        public static void ExtractMarkupAndChildOffsetsExt(this ICollection<PageComponent> scaffoldingComponents, ScaffoldAdapter scaffold)
        {
            var que = new Queue<ScaffoldingItem>();
            var childOff = new List<ChildOffset>();
            int it = 0;
            foreach(var a in scaffoldingComponents.OrderBy(x => x.SelfPageOrder))
            {

                que.Enqueue(new(a.Id, a.ComponentHtml, new ChildOffset(a.Id, a.ChildOffset)));
            }
            scaffold.ComponentHtml = new Queue<ScaffoldingItem>(que.ToList());
        }
        private static string Interpolateheaders(IComponentPeripheral value, Func<IComponentPeripheral, string?> propt)
        {
            return $"{(propt(value) == null  ? "" :  $"{propt(value)}\n")}";
        }
    }
}
