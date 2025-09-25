using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace CMSProj.SubSystems.Identity
{
    public sealed class AdminAssetProvider : IAdminAssetProvider
    {
        private readonly IHttpContextAccessor _http;
        private readonly IFileVersionProvider _versioner;

        public AdminAssetProvider(IHttpContextAccessor http, IFileVersionProvider versioner)
        {
            _http = http;
            _versioner = versioner;
        }

        public IHtmlContent? RenderAdminScript(string virtualPath = "~/js/lib/admin/adminscript.js")
        {
            var httpCtx = _http.HttpContext;
            var user = httpCtx?.User;

            if (user?.Identity?.IsAuthenticated != true || !user.IsInRole("Admin"))
                return null;

            var versioned = _versioner.AddFileVersionToPath(httpCtx.Request.Path, virtualPath);

            var tag = new TagBuilder("script");
            tag.Attributes["src"] = versioned;
            return tag;
        }
    }
}
