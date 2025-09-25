using CMSProj.DataLayer.PageServices.Components;

namespace CMSProj.SubSystems.Publishing
{
    public interface IContentBuilder
    {
        IRenderContent BuildContent(PageAdapter adapter);
    }
}

