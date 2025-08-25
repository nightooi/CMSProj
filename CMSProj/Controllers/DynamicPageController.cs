using System.Buffers;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Net;
using System.Security.Principal;

using CMSProj.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32.SafeHandles;

namespace CMSProj.Controllers
{
    public class DynamicPageController : Controller
    {
        private readonly ILogger<DynamicPageController> _logger;
        private readonly IContentRepository _cmsRepo;
        public DynamicPageController(ILogger<DynamicPageController> logger, IContentRepository repo)
        {
            _logger = logger;
            _cmsRepo = repo;
        }
        /// <summary>
        /// if this correctly uses a task pattern, the call to admin/endclient is opaque at this layer, it should get injected into the controller insantiation 
        /// at request time.
        /// Extra dependency to the controller -> not sure what yet.
        /// </summary>
        /// <param name="pageGuid"></param>
        /// <returns></returns>
        [HttpGet(Name = "{pageGuid}")]
        public IActionResult RenderPage([FromRoute(Name = "pageGuid")] string pageGuid)
        {
            var pageExcavation = _cmsRepo.GetPageContent(new Guid(pageGuid));
            var content = new RenderContent() { Content = pageExcavation };

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

