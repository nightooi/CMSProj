using Microsoft.AspNetCore.Html;

namespace CMSProj.SubSystems.Identity
{
    public interface IAdminAssetProvider
    {
        IHtmlContent? RenderAdminScript(string virtualPath = "~/js/lib/admin/adminscript.js");
    }
}
