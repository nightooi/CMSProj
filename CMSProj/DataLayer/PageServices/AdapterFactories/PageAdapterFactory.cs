using CMSProj.DataLayer.DatalayerExtensions;
using CMSProj.DataLayer.PageServices.Components;

namespace CMSProj.DataLayer.PageServices.AdapterFactories
{
    public class PageAdapterFactory : IDatalayerFactory<PageAdapter, ContentDatabase.Model.Page>
    {
        public PageAdapter Create(ContentDatabase.Model.Page model)
        {
            return new PageAdapter(model.Id, model.Published)
            {
                Scaffolding = new(model.PageVersions.First().PageTemplate.Id, DateTime.UtcNow),
                Content = new(),
                Html = new(),
                Slug = model.Slug.Slug,
                HeaderContents = string.Empty,
                JsContents = string.Empty,
                PageName = model.PageName,
                PageContent = (IReadOnlyCollection<ContentComponent>)model.PageVersions.First().Components.ExtractAssetsExt(new ContentComponentFactory().Create),
            };
        }
    }
}
