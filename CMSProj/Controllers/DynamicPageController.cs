using System.Buffers;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Net;
using System.Security.Principal;

using CMSProj.DataLayer;
using CMSProj.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32.SafeHandles;

namespace CMSProj.Controllers
{
    public class DynamicPageController : Controller
    {
        private readonly ILogger<DynamicPageController> _logger;
        private readonly IPageRepository _cmsRepo;
        private readonly IContentBuilder _builder;
        public DynamicPageController(ILogger<DynamicPageController> logger, IPageRepository repo, IContentBuilder builder)
        {
            _logger = logger;
            _cmsRepo = repo;
            _builder = builder;
        }
        /// <summary>
        /// if this correctly uses a task pattern, the call to admin/endclient is opaque at this layer, it should get injected into the controller insantiation 
        /// at request time.
        /// Extra dependency to the controller -> not sure what yet.
        /// </summary>
        /// <param name="pageGuid"></param>
        /// <returns></returns>
        [HttpGet(Name = "{pageGuid}")]
        public async Task<IActionResult> RenderPage([FromRoute(Name = "pageGuid")] string pageGuid, CancellationToken token)
        {
            var guid = Guid.Parse(pageGuid);
            var pageExcavation = await _cmsRepo.ScaffoldPageAsync(guid, token);
            var assets = _cmsRepo.CopyAssetsToServingDirectory(guid, token);
            var page = await _cmsRepo.ConstructCompletePageAsync(pageExcavation, guid, token);
            var content =_builder.BuildContent(page);

            return View(content);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet(Name = "{pageGuid}")]
        [HttpGet(Name = "{params}")]
        public IActionResult RenderParameteredPage(
            [FromRoute(Name = "pageGuid")] string? pageGuid,
            [FromRoute(Name = "params")] params object[]? values)
        {
            return View();
        }
    }
}

