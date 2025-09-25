using CMSProj.DataLayer.PageServices.Components;

using ContentDatabase.Model;
namespace CMSProj.DataLayer.PageServices.AdapterFactories
{
    public class AssetFactory : IDatalayerFactory<AssetAdapter, Assets>
    {

        public AssetAdapter Create(Assets model)
        {
            return new AssetAdapter(model.Id, model.Published)
            {
                Uri = model.Url,
                AssetFiletype = model.AssetFileType.FileType,
                Content = null,
                HeaderContents = null,
            };
        }
    }
}
