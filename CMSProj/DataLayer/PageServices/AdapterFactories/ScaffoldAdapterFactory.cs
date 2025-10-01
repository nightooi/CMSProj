using CMSProj.DataLayer.DatalayerExtensions;
using CMSProj.DataLayer.PageServices.Components;

using ContentDatabase.Model;
namespace CMSProj.DataLayer.PageServices.AdapterFactories
{
    public class ScaffoldAdapterFactory : IDatalayerFactory<ScaffoldAdapter, PageTemplate>
    {
        public ScaffoldAdapter Create(PageTemplate model)
        {
            var peripherals = model.PageComponents.ExtractPeripheralsExt();
            var scaffold = new ScaffoldAdapter(model.Id, model.Published)
            {
                HeaderContents = $"{peripherals.HeaderContents}\n{peripherals.JsContents}",
                JsContents = $"{peripherals.JsContents}",
                ComponentHtml = [],
            };

            model.PageComponents.ExtractMarkupAndChildOffsetsExt(scaffold);
            return scaffold;
        }
    }
}
