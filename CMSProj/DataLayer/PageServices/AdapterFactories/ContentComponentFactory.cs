using CMSProj.DataLayer.DatalayerExtensions;
using CMSProj.DataLayer.PageServices.Components;

using ContentDatabase.DeserliazationTypes;
using ContentDatabase.Model;

using System.Text.Json;
namespace CMSProj.DataLayer.PageServices.AdapterFactories
{
    public class ContentComponentFactory : IDatalayerFactory<ContentComponent, AuthoredComponent>
    {
        public ContentComponent Create(AuthoredComponent model)
        {
            return new ContentComponent(model.Id, model.Published)
            {
                Assets = (IReadOnlyCollection<AssetAdapter>)model.Assets.ExtractAssetsExt(new AssetFactory().Create),
                HtmlMarkup = model.PayLoad.Markup,
                HeaderContents = $"{model.OtherHeaders}\n {model.CssHeaderTags}\n {model.JsHeaderTags}",
                ScaffoldAdapterId = model.PageComponentId ?? throw new ArgumentNullException("oopsieee... we have orphaned entries :)"),
                JsContents = model.JsBodyTags,
            };
        }
    }
}
