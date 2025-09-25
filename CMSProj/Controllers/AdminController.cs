using CMSProj.DataLayer.PageServices.Repo;
using CMSProj.SubSystems.Publishing;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using NuGet.Common;

namespace CMSProj.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IPageRepository _cmsRepo;
        private readonly IContentBuilder _builder;
        public AdminController(
            ILogger<AdminController> logger,
            IPageRepository pagerepo,
            IContentBuilder builder)
        {
            _logger = logger;
            _cmsRepo = pagerepo;
            _builder = builder;
        }


        [Authorize(Roles ="Admin")]
        [ResponseCache(Location = ResponseCacheLocation.Client, NoStore = true)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> RenderPageAdmin([FromRoute(Name = "pageGuid")] Guid pageGuid)
        {
            var pageExcavation = await _cmsRepo.ScaffoldPageAsync(pageGuid, HttpContext.RequestAborted);
            var assets = _cmsRepo.CopyAssetsToServingDirectory(pageGuid, HttpContext.RequestAborted);
            var page = await _cmsRepo.ConstructCompletePageAsync(pageExcavation, pageGuid, HttpContext.RequestAborted);
            var content = _builder.BuildContent(page);
            ViewBag.RenderContent = content;

            return View();
        }
    }
}
