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
        private readonly DataLayer.PageServices.Repo.IPageManager _cmsRepo;
        private readonly IContentBuilder _builder;
        private readonly ICounterModelFactory _counterFactory;
        private readonly IWebHostEnvironment _env;
        ICounterApi _counterApi;

        public AdminController(
            ILogger<AdminController> logger,
            DataLayer.PageServices.Repo.IPageManager pagerepo,
            IContentBuilder builder,
            ICounterModelFactory counterfact, 
            ICounterApi counterapi, 
            IWebHostEnvironment env)
        {
            _logger = logger;
            _cmsRepo = pagerepo;
            _builder = builder;
            _counterFactory = counterfact;
            _counterApi = counterapi;
            _env = env;
        }


        [Authorize(Roles ="Admin")]
        [ResponseCache(Location = ResponseCacheLocation.Client, NoStore = true)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> RenderPageAdmin([FromRoute(Name = "pageGuid")] Guid pageGuid)
        {
            //var key = HttpContext.Request?.Path.Value?.Trim('/') ?? "home";
            //var count = await _counterApi.GetCountAsync(key) ?? 0;
            //var result = _counterFactory.Create(key, count);

            var pageExcavation = await _cmsRepo.ScaffoldPageAsync(pageGuid, HttpContext.RequestAborted);
            var assets = _cmsRepo.CopyAssetsToServingDirectory(pageGuid, HttpContext.RequestAborted);
            var page = await _cmsRepo.ConstructCompletePageAsync(pageExcavation, pageGuid, HttpContext.RequestAborted);
            var content = _builder.BuildContent(page);
            ViewBag.RenderContent = content;

            return View();
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
    }
}
