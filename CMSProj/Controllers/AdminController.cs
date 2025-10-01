using CMSProj.DataLayer.PageServices.Repo;
using CMSProj.Model;
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
        private readonly DataLayer.PageServices.Repo.IPageManager _cmsRepo;
        private readonly IContentBuilder _builder;
        private readonly IWebHostEnvironment _env;
        private readonly ICounterApiManager _counter;
        private readonly IContactManager _mgr;
        public AdminController(
            ILogger<AdminController> logger,
            DataLayer.PageServices.Repo.IPageManager pagerepo,
            IContentBuilder builder,
            IWebHostEnvironment env, 
            ICounterApiManager counter,
            IContactManager mgr)
        {
            _logger = logger;
            _cmsRepo = pagerepo;
            _builder = builder;
            _counter = counter;
            _env = env;
            _mgr = mgr;
        }


        [Authorize(Roles ="Admin")]
        [ResponseCache(Location = ResponseCacheLocation.Client, NoStore = true)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> RenderPageAdmin([FromRoute(Name = "pageGuid")] Guid pageGuid)
        {

            var result = await _counter.CurrentPageCounter(HttpContext);
            //this aught to have been wrapped in a manager.
            var pageExcavation = await _cmsRepo.ScaffoldPageAsync(pageGuid, HttpContext.RequestAborted);
            var assets = _cmsRepo.CopyAssetsToServingDirectory(pageGuid, HttpContext.RequestAborted);
            var page = await _cmsRepo.ConstructCompletePageAsync(pageExcavation, pageGuid, HttpContext.RequestAborted);
            var content = _builder.BuildContent(page);
            ViewBag.RenderContent = content;
            ViewBag.FormModel = new ContactFormVm();

            return View(result);
        }
        [HttpGet]
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var list = await _mgr.GetAllAsync(ct);
            return View(list);
        }
        [HttpPost]
        [Authorize(Roles ="Admin")]
        [RequestSizeLimit(50000000)]
        [ApiExplorerSettings(IgnoreApi =true)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ImageUpload(IFormFile file)
        {
            if (file is null || file.Length == 0) return BadRequest("No file.");

            var permitted = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!permitted.Contains(ext)) return BadRequest("Unsupported type.");

            var uploads = Path.Combine(_env.WebRootPath!, $"public");
            Directory.CreateDirectory(uploads);

            var name = $"{Guid.NewGuid():N}{ext}";
            var path = Path.Combine(uploads, name);

            await using var fs = System.IO.File.Create(path);
            await file.CopyToAsync(fs);

            return Created($"/public/{name}", new { url = $"/public/{name}" });
        }
        [HttpPost]
        [Authorize(Roles="Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(Guid id, CancellationToken ct)
        {
            await _mgr.RemoveAsync(id, ct);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult ContactFormPartial() => PartialView("ContactAdmin", new ContactFormVm());
    }
}
