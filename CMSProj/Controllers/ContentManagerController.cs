using CMSProj.DataLayer.DatalayerExtensions;

using ContentDatabase;
using ContentDatabase.DeserliazationTypes;
using ContentDatabase.Model;

using HtmlAgilityPack;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Runtime.CompilerServices;
using CMSProj.Controllers;
using System.Text;
using System.Text.Json.Serialization.Metadata;

namespace CMSProj.Controllers
{
    public interface IDefaultPageCreator
    {
        Task<Page> CreateAsync(CancellationToken ct);
    }

    public sealed class DefaultPageCreator : IDefaultPageCreator
    {
        private readonly ContentContext _db;

        public DefaultPageCreator(ContentContext db)
            => _db = db;

        public async Task<Page> CreateAsync(CancellationToken ct = default)
        {
            var now = DateTime.UtcNow;

            var systemAuthor = await _db.Authors.FirstOrDefaultAsync(a => a.Name == "System", ct);
            if (systemAuthor is null)
            {
                systemAuthor = new Author
                {
                    Name = "System",
                    ContactEmail = "system@example.com",
                    PageComponents = new List<PageComponent>(),
                    PageTemplates = new List<PageTemplate>()
                };
                _db.Authors.Add(systemAuthor);
                await _db.SaveChangesAsync(ct);
            }

            var cssAsset = await _db.Assets.FirstOrDefaultAsync(a => a.AssetName == "Base CSS", ct);
            var jsAsset = await _db.Assets.FirstOrDefaultAsync(a => a.AssetName == "App JS", ct);

            if (cssAsset is null || jsAsset is null)
            {
                var cssType = await GetOrCreateFileTypeAsync("text/css", ct);
                var jsType = await GetOrCreateFileTypeAsync("text/javascript", ct);
                var host = await GetOrCreateHostAsync("Local", "https://localhost://", ct);

                cssAsset ??= new Assets
                {
                    Url = "https://cdn.example.com/styles/base.css",
                    AssetName = "Base CSS",
                    AssetDescription = "Base stylesheet",
                    FileType = cssType.FileType,
                    AssetFileType = cssType,
                    AssetDomain = host,
                    Constructed = now,
                    Generated = now,
                    Published = now,
                    CopyRight = "© Example",
                    CreationAuthor = systemAuthor,
                    LastRevisionTime = now,
                    RevisionAuthor = new List<Author> { systemAuthor }
                };

                jsAsset ??= new Assets
                {
                    Url = "https://cdn.example.com/scripts/app.js",
                    AssetName = "App JS",
                    AssetDescription = "Frontend logic",
                    FileType = jsType.FileType,
                    AssetFileType = jsType,
                    AssetDomain = host,
                    Constructed = now,
                    Generated = now,
                    Published = now,
                    CopyRight = "© Example",
                    CreationAuthor = systemAuthor,
                    LastRevisionTime = now,
                    RevisionAuthor = new List<Author> { systemAuthor }
                };

                _db.Assets.AddRange(cssAsset, jsAsset);
                await _db.SaveChangesAsync(ct);
            }

            var defaultTemplate = new PageTemplate
            {
                Version = 1,
                PageVersions = new List<PageVersion>(),
                Constructed = now,
                Generated = now,
                Published = now,
                CopyRight = "© Example",
                CreationAuthor = systemAuthor,
                LastRevisionTime = now,
                RevisionAuthor = new List<Author> { systemAuthor }
            };

            var baseComponentHeader = new PageComponent
            {
                ComponentHtml = "<header class=\"navbar navbar-expand-lg navbar-dark bg-primary fixed-top\">\r\n \r\n</header>",
                PageTemplate = defaultTemplate,
                ChildOffset = 78,
                SelfPageOrder = 0,
                Constructed = now,
                Generated = now,
                Published = now,
                CopyRightDisclaimer = null,
                CreationAuthor = systemAuthor,
                LastRevisionTime = now,
                RevisionDiff = null,
                RevisionAuthor = new List<Author> { systemAuthor }
            };
            var baseComponentBod1 = new PageComponent
            {
                ComponentHtml = "<section class=\"jumbotron jumbotron-fluid bg-light text-center mb-0\">\r\n      \r\n    </section>",
                PageTemplate = defaultTemplate,
                ChildOffset = 75,
                SelfPageOrder = 1,
                Constructed = now,
                Generated = now,
                Published = now,
                CopyRightDisclaimer = null,
                CreationAuthor = systemAuthor,
                LastRevisionTime = now,
                RevisionDiff = null,
                RevisionAuthor = new List<Author> { systemAuthor }
            };
            var baseComponentBod2 = new PageComponent
            {
                ComponentHtml = "<section id=\"about\" class=\"container py-5\">\r\n        \r\n    </section>",
                PageTemplate = defaultTemplate,
                ChildOffset = 51,
                SelfPageOrder = 2,
                Constructed = now,
                Generated = now,
                Published = now,
                CopyRightDisclaimer = null,
                CreationAuthor = systemAuthor,
                LastRevisionTime = now,
                RevisionDiff = null,
                RevisionAuthor = new List<Author> { systemAuthor }
            };
            var baseComponentBod3 = new PageComponent
            {
                ComponentHtml = "<section id=\"services\" class=\"container py-5\">\r\n        \r\n    </section>",
                PageTemplate = defaultTemplate,
                ChildOffset = 54,
                SelfPageOrder = 3,
                Constructed = now,
                Generated = now,
                Published = now,
                CopyRightDisclaimer = null,
                CreationAuthor = systemAuthor,
                LastRevisionTime = now,
                RevisionDiff = null,
                RevisionAuthor = new List<Author> { systemAuthor }
            };

            var baseComponentBod4 = new PageComponent
            {
                ComponentHtml = "<footer class=\"text-center py-4 bg-dark text-white\">\r\n       \r\n    </footer>",
                PageTemplate = defaultTemplate,
                ChildOffset = 58,
                SelfPageOrder = 4,
                Constructed = now,
                Generated = now,
                Published = now,
                CopyRightDisclaimer = null,
                CreationAuthor = systemAuthor,
                LastRevisionTime = now,
                RevisionDiff = null,
                RevisionAuthor = new List<Author> { systemAuthor }
            };

            _db.PageTemplates.Add(defaultTemplate);
            _db.PageComponents.AddRange(baseComponentHeader, baseComponentBod1, baseComponentBod2, baseComponentBod3, baseComponentBod4);

            var c1 = "<small>&copy; 2025 GenericCompany. All rights reserved.</small>";
            var c2 = "<h2 class=\"mb-4\">Services</h2>\r\n\r\n        <div data-role=\"collapsible-set\" data-theme=\"b\">\r\n            <div data-role=\"collapsible\">\r\n                <h3>Consulting</h3>\r\n                <p>Strategy, architecture reviews, and proof-of-concepts.</p>\r\n            </div>\r\n            <div data-role=\"collapsible\">\r\n                <h3>Development</h3>\r\n                <p>Full-stack web &amp; mobile apps built on modern tech.</p>\r\n            </div>\r\n            <div data-role=\"collapsible\">\r\n                <h3>Support</h3>\r\n                <p>24×7 monitoring, SLAs, and incident response.</p>\r\n            </div>\r\n        </div>";
            var c3 = "<h2>About Us</h2>\r\n        <p>\r\n            GenericCompany has delivered quality widgets and services since 2025.\r\n            Our team focuses on customer success, agile delivery, and coffee-fueled creativity.\r\n        </p>";
            var c4 = "<div class=\"container\">\r\n            <h1 class=\"display-4\">Welcome to GenericCompany</h1>\r\n            <p class=\"lead\">Your one-stop shop for utterly versatile solutions.</p>\r\n            <a href=\"#services\" class=\"btn btn-primary btn-lg\">What we do</a>\r\n        </div>";
            var c5 = "<a class=\"navbar-brand\" href=\"#\">GenericCompany</a>\r\n        <button class=\"navbar-toggler\" type=\"button\" data-toggle=\"collapse\"\r\n                data-target=\"#mainNav\" aria-controls=\"mainNav\"\r\n                aria-expanded=\"false\" aria-label=\"Toggle navigation\">\r\n            <span class=\"navbar-toggler-icon\"></span>\r\n        </button>\r\n\r\n        <nav id=\"mainNav\" class=\"collapse navbar-collapse\">\r\n            <ul class=\"navbar-nav ml-auto\">\r\n                <li class=\"nav-item\"><a class=\"nav-link\" href=\"#about\">About</a></li>\r\n                <li class=\"nav-item\"><a class=\"nav-link\" href=\"#services\">Services</a></li>\r\n                <li class=\"nav-item\"><a class=\"nav-link\" href=\"#contact\">Contact</a></li>\r\n            </ul>\r\n        </nav>";

            var cmpc5 = IComponentMarkupFactory.Create(c5);

            var cmpc1 = IComponentMarkupFactory.Create(c1);
            var cmpc2 = IComponentMarkupFactory.Create(c2);
            var cmpc3 = IComponentMarkupFactory.Create(c3);
            var cmpc4 = IComponentMarkupFactory.Create(c4);

            var defaultPayl1 = NewPayload(cmpc1, systemAuthor, now);
            var defaultPayl2 = NewPayload(cmpc2, systemAuthor, now);
            var defaultPayl3 = NewPayload(cmpc3, systemAuthor, now);
            var defaultPayl4 = NewPayload(cmpc4, systemAuthor, now);
            var defaultPayl5 = NewPayload(cmpc5, systemAuthor, now);

            _db.ComponentMarkups.AddRange(defaultPayl1, defaultPayl2, defaultPayl3, defaultPayl4, defaultPayl5);

            var page = new Page
            {
                PageName = $"DefaultPage",
                PageVersions = new List<PageVersion>(),
                Constructed = now,
                Generated = now,
                Published = now,
                CopyRight = "© Example",
                CreationAuthor = systemAuthor,
                LastRevisionTime = now,
                RevisionAuthor = new List<Author> { systemAuthor }
            };
            _db.Pages.Add(page);

            var defaultVersion = new PageVersion
            {
                Version = 1,
                Components = new List<AuthoredComponent>(),
                PageTemplate = defaultTemplate,
                Page = page,
                Constructed = now,
                Generated = now,
                Published = now,
                CopyRight = "© Example",
                CreationAuthor = systemAuthor,
                LastRevisionTime = now,
                RevisionAuthor = new List<Author> { systemAuthor }
            };
            _db.PageVersions.Add(defaultVersion);

            var authored1 = NewAuthored(defaultPayl5, "Nav.Header.v1", baseComponentHeader, defaultVersion, cssAsset.Url, jsAsset.Url, systemAuthor, now);
            var authored2 = NewAuthored(defaultPayl4, "Hero.Section.v1", baseComponentBod1, defaultVersion, cssAsset.Url, jsAsset.Url, systemAuthor, now);
            var authored3 = NewAuthored(defaultPayl3, "About.Section.v1", baseComponentBod2, defaultVersion, cssAsset.Url, jsAsset.Url, systemAuthor, now);
            var authored4 = NewAuthored(defaultPayl2, "Services.Section.v1", baseComponentBod3, defaultVersion, cssAsset.Url, jsAsset.Url, systemAuthor, now);
            var authored5 = NewAuthored(defaultPayl1, "Footer.Section.v1", baseComponentBod4, defaultVersion, cssAsset.Url, jsAsset.Url, systemAuthor, now);

            _db.AuthoredComponents.AddRange(authored1, authored2, authored3, authored4, authored5);

            await _db.SaveChangesAsync(ct);
            return page;

            ComponentMarkup NewPayload(dynamic cmp, Author author, DateTime timestamp)
                => new ComponentMarkup
                {
                    Constructed = timestamp,
                    Markup = JsonSerializer.Serialize(cmp.Markup),
                    Content = JsonSerializer.Serialize(cmp.Content),
                    Generated = timestamp,
                    CopyRight = "© Example",
                    CreationAuthor = author,
                    LastRevisionTime = timestamp,
                    RevisionAuthor = new List<Author> { author }
                };

            AuthoredComponent NewAuthored(
                ComponentMarkup payload, string name, PageComponent slot, PageVersion version,
                string cssUrl, string jsUrl, Author author, DateTime timestamp)
                => new AuthoredComponent
                {
                    PayLoad = payload,
                    ComponentName = name,
                    CssHeaderTags = cssUrl,
                    JsHeaderTags = jsUrl,
                    JsBodyTags = null,
                    PageVersion = version,
                    PageComponent = slot,
                    Constructed = timestamp,
                    Generated = timestamp,
                    Published = timestamp,
                    CopyRight = "© Example",
                    CreationAuthor = author,
                    LastRevisionTime = timestamp,
                    RevisionAuthor = new List<Author> { author }
                };
        }

        private async Task<AssetFileType> GetOrCreateFileTypeAsync(string mime, CancellationToken ct)
        {
            var ft = await _db.AssetFileTypes.FirstOrDefaultAsync(x => x.FileType == mime, ct);
            if (ft is null)
            {
                ft = new AssetFileType { Assets = new List<Assets>(), FileType = mime };
                _db.AssetFileTypes.Add(ft);
                await _db.SaveChangesAsync(ct);
            }
            return ft;
        }

        private async Task<AssetHostDomain> GetOrCreateHostAsync(string name, string url, CancellationToken ct)
        {
            var host = await _db.AssetHostDomains.FirstOrDefaultAsync(x => x.DomainName == name, ct);
            if (host is null)
            {
                host = new AssetHostDomain { DomainName = name, DomainUrl = url, Assets = new List<Assets>() };
                _db.AssetHostDomains.Add(host);
                await _db.SaveChangesAsync(ct);
            }
            return host;
        }
    }

    public static class PageManagementExt
    {
        public static IServiceCollection AddPageManagement(this IServiceCollection collection)
        {
            collection.AddScoped<IPageContentManager, PageContentManager>();
            collection.AddScoped<ISlugFactory, SlugFactory>();
            collection.AddScoped<IDefaultsRepo, DefaultsCreator>();
            collection.AddScoped<IDefaultPageCreator, DefaultPageCreator>();
            collection.AddScoped<ISubComponentManager, SubComponentManager>();


            return collection;
        }
    }
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class ContentManagerController : ControllerBase
    {

        private IPageContentManager _pageManager { get; }
        private ILogger<ContentManagerController> _logger { get; }
        private ISubComponentManager _subManager { get; } // <-- add

        public ContentManagerController(
            IPageContentManager pageManager,
            ILogger<ContentManagerController> logger,
            ISubComponentManager subManager) // <-- add
        {
            _pageManager = pageManager;
            _logger = logger;
            _subManager = subManager; // <-- add
        }
        /// <summary>
        /// Creates a Default Page at given url
        /// </summary>
        /// <param name="page">Required! maxlenght of 2000 characters</param>
        /// <returns>200</returns>
        /// <returns>201</returns>
        /// <returns>400</returns>
        /// <returns>401</returns>
        /// <returns>422</returns>
        /// <returns>500</returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreatePage(PageDetails page)
        {
            if (User is null || !User.IsInRole("Admin"))
                return Unauthorized();

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            _logger.LogInformation($"{User.Identity?.Name ?? "defaultAdmin"} Issued Page Creation: {DateTime.UtcNow}");
            var res = _pageManager.CreatePageAsync(page, HttpContext.RequestAborted);
            await res;
            if (res.IsFaulted)
            {
                _logger.LogError(res.Exception, $"Creation Failed: {DateTime.UtcNow} with Exception: {res.Exception}");
                return BadRequest(new
                {
                    result = new
                    {
                        Slug = page.Url,
                        ExcMessage = $"{page.Url} is likely taken.\n" +
                    $" Process exited with fault{res.Exception}"
                    },
                });
            }
            if (res.IsCanceled)
            {
                _logger.LogInformation($"Creation Failed: {DateTime.UtcNow}: Cancelled by user");
                return Ok();
            }
            if (res.IsCompletedSuccessfully && (await res) == DBActionResult.Succeded)
            {
                _logger.LogInformation($"Resource Created: {DateTime.UtcNow}, with Name: {page.Url}");
                return Ok(new
                {
                    available = DateTime.UtcNow.AddMinutes(5)

                });
            }
            _logger.LogCritical($"Server Failed to create resource but did not throw en Error! Time: {DateTime.UtcNow}");
            return StatusCode(500);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletePage(PageDetails page)
        {
            if (User is null || !User.IsInRole("Admin"))
                return Unauthorized();

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var t = _pageManager.DeletePageAsync(page, Request.HttpContext.RequestAborted);
            var res = await t;

            if (t.IsFaulted)
                return BadRequest(new
                {
                    request = nameof(ContentManagerController),
                    action = nameof(ContentManagerController.DeletePage),
                    result_action = res,
                    exc = t.Exception?.Message ?? "Process did not throw an exception, but the execution failed"
                });

            if (t.IsCanceled)
                return Ok(new
                {
                    request = nameof(ContentManagerController),
                    action = nameof(ContentManagerController.DeletePage),
                    result_action = DBActionResult.Cancelled.ToString(),
                });

            if (t.IsCompletedSuccessfully && res == DBActionResult.Succeded)
                return Ok(new
                {
                    request = nameof(ContentManagerController),
                    action = nameof(ContentManagerController.DeletePage),
                    result_action = res,
                    url = page.Url,
                    url_delisting = DateTime.UtcNow.AddMinutes(5)
                });

            if (t.IsCompleted && res.HasFlag(DBActionResult.NotFound))
                return NotFound(new
                {
                    request = nameof(ContentManagerController),
                    action = nameof(ContentManagerController.DeletePage),
                    url = page.Url,
                    message = "Specified resource does not exist."
                });

            if (t.IsCompletedSuccessfully && (res.HasFlag(DBActionResult.Failed) || res.HasFlag(DBActionResult.Error)))
                return StatusCode(500, new
                {
                    request = nameof(ContentManagerController),
                    action = nameof(ContentManagerController.DeletePage),
                    result_action = res,
                    url = page.Url,
                    message = $"Request processed, the resource removal failed without throwing any Error"
                });

            return StatusCode(501, new
            {
                request = nameof(ContentManagerController),
                action = nameof(ContentManagerController.DeletePage),
                result_action = res,
                url = page.Url,
                message = $"Internal Error"
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EditPage(PageEdit page)
        {
            if (User is null || !User.IsInRole("Admin"))
                return Unauthorized();

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var list = new List<PageUpdates>();
            int iterator = 0;
            //lets hope for no deadlocks
            var updates = await _pageManager.UpdatePageAsync(page, HttpContext.RequestAborted);

            var totalRes = DBActionResult.Succeded;
            try
            {
                if (HttpContext.RequestAborted.IsCancellationRequested)
                {
                    return Ok(new
                    {
                        request = nameof(ContentManagerController),
                        action = nameof(ContentManagerController.EditPage),
                        status = DBActionResult.Cancelled.ToString(),
                        processed_overview = totalRes.ToString(),
                        processed = list
                    });
                }
                var item = page.ContentEdits.ElementAt(iterator);
                list.Add(new()
                {
                    element_guid = item.ComponentGuid ?? new Guid(),
                    edit_action = page.ContentEdits.ElementAt(iterator).Choice,
                    exception_message = "",
                    result = updates
                });
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, exc.Message);

                if (exc is ArgumentNullException)
                    return UnprocessableEntity(new
                    {
                        request = nameof(ContentManagerController),
                        action = nameof(ContentManagerController.EditPage),
                        status = DBActionResult.Failed,
                        exception_message = "Page Does not exist!"
                    });

                list.Add(new PageUpdates()
                {
                    element_guid = page.ContentEdits.ElementAt(iterator).ComponentGuid ?? new Guid(),
                    exception_message = exc.Message ?? "Task Threw but did not leave an exception",
                    result = new()
                    {
                        result = DBActionResult.Error,
                        value_result = string.Empty
                    }
                });
            }

            return Ok(new
            {
                request = nameof(ContentManagerController),
                action = nameof(ContentManagerController.EditPage),
                status = DBActionResult.Cancelled.ToString(),
                processed_overview = totalRes.ToString(),
                processed = list
            });
        }
        [Route("SubComp")]
        [Authorize(Roles = "Admin")]
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EditSubComponent([FromBody]SubComponentEdit edit)
        {
            if (User is null || !User.IsInRole("Admin"))
                return Unauthorized();

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            try
            {
                if (HttpContext.RequestAborted.IsCancellationRequested)
                    return Ok(new
                    {
                        request = nameof(ContentManagerController),
                        action = nameof(ContentManagerController.EditSubComponent),
                        result_action = DBActionResult.Cancelled.ToString(),
                        url = edit.url
                    });

                Task<UpdateResults> t = edit.Choice switch
                {
                    EditChoice.Add => _subManager.AddSubComponentAsync(edit, HttpContext.RequestAborted),
                    EditChoice.Remove => _subManager.DeleteSubComponentAsync(edit, HttpContext.RequestAborted),
                    EditChoice.Edit => _subManager.EditSubComponentAsync(edit, HttpContext.RequestAborted),
                    EditChoice.Menu => _subManager.EditSubComponentAsync(edit, HttpContext.RequestAborted),
                    _ => Task.FromResult(IUpdateResultCreator.Failed())
                };

                var res = await t;

                if (t.IsFaulted)
                    return BadRequest(new
                    {
                        request = nameof(ContentManagerController),
                        action = nameof(ContentManagerController.EditSubComponent),
                        result_action = res.result,
                        exc = t.Exception?.Message ?? "Task faulted but did not throw an exception"
                    });

                if (t.IsCanceled || res.result == DBActionResult.Cancelled)
                    return Ok(new
                    {
                        request = nameof(ContentManagerController),
                        action = nameof(ContentManagerController.EditSubComponent),
                        result_action = DBActionResult.Cancelled.ToString(),
                        url = edit.url
                    });

                if (res.result == DBActionResult.Succeded)
                    return Ok(new
                    {
                        request = nameof(ContentManagerController),
                        action = nameof(ContentManagerController.EditSubComponent),
                        result_action = res.result,
                        url = edit.url,
                        value_result = res.value_result
                    });

                if (res.result == DBActionResult.NotFound)
                    return NotFound(new
                    {
                        request = nameof(ContentManagerController),
                        action = nameof(ContentManagerController.EditSubComponent),
                        url = edit.url,
                        message = "Specified resource does not exist."
                    });

                if (res.result == DBActionResult.Failed || res.result == DBActionResult.Error)
                    return StatusCode(500, new
                    {
                        request = nameof(ContentManagerController),
                        action = nameof(ContentManagerController.EditSubComponent),
                        result_action = res.result,
                        url = edit.url,
                        message = "Request processed, but the update failed."
                    });

                return StatusCode(501, new
                {
                    request = nameof(ContentManagerController),
                    action = nameof(ContentManagerController.EditSubComponent),
                    result_action = res.result,
                    url = edit.url,
                    message = "Internal Error"
                });
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, exc.Message);
                return StatusCode(500, new
                {
                    request = nameof(ContentManagerController),
                    action = nameof(ContentManagerController.EditSubComponent),
                    result_action = DBActionResult.Error,
                    url = edit.url,
                    message = exc.Message
                });
            }
        }
    }

    public interface ISubComponentManager
    {
        Task<UpdateResults> AddSubComponentAsync(SubComponentEdit edit, CancellationToken ct);
        Task<UpdateResults> EditSubComponentAsync(SubComponentEdit edit, CancellationToken ct);
        Task<UpdateResults> DeleteSubComponentAsync(SubComponentEdit edit, CancellationToken ct);
    }

    public class SubComponentManager : ISubComponentManager
    {
        private readonly ContentContext _ctx;
        private readonly IMenuManger _menumgr;

        public SubComponentManager(ContentContext ctx, IMenuManger menumgr)
        {
            _ctx = ctx;
            _menumgr = menumgr;
        }

        public Task<UpdateResults> AddSubComponentAsync(SubComponentEdit edit, CancellationToken ct)
            => UpsertAsync(edit, ct);

        public Task<UpdateResults> EditSubComponentAsync(SubComponentEdit edit, CancellationToken ct)
            => UpsertAsync(edit, ct);

        public async Task<UpdateResults> DeleteSubComponentAsync(SubComponentEdit edit, CancellationToken ct)
        {
            //client sends the full updated root markup WITHOUT the subcomponent.
            if (ct.IsCancellationRequested)
                return IUpdateResultCreator.Cancelled(string.Empty);

            var updated = await UpsertAsync(edit, ct, validateDelete: true);
            return updated;
        }

        private async Task<UpdateResults> UpsertAsync(SubComponentEdit edit, CancellationToken ct, bool validateDelete = false)
        {
            if (ct.IsCancellationRequested)
                return IUpdateResultCreator.Cancelled(edit.value);

            if (edit.Choice.HasFlag(EditChoice.Menu))
                return await _menumgr.EditMenuAsync(edit.value);
         
            var slug = await IRequestContext.GetLoadedSlug(_ctx, edit.url);
            if (slug is null)
                return IUpdateResultCreator.SlugNull();

            var root = slug.PageVersion!
                .PageTemplate!
                .PageComponents!
                .SingleOrDefault(x => x.Id == edit.cms_root_component);

            if (root is null)
            {
                root = await _ctx.PageComponents.SingleOrDefaultAsync(x => x.Id == edit.cms_root_component, ct);
                if (root is null)
                    return new UpdateResults { result = DBActionResult.NotFound, value_result = string.Empty };
            }

            var authored = await _ctx.AuthoredComponents
                .Where(x => x.PageVersionId == slug.PageVersionId)
                .Where(x => x.PageComponentId == root.Id)
                .Include(x => x.PayLoad)
                .SingleOrDefaultAsync(ct);

            if (authored is null)
                return new UpdateResults { result = DBActionResult.NotFound, value_result = string.Empty };

            if (string.IsNullOrWhiteSpace(edit.value))
                return IUpdateResultCreator.Failed();

            try
            {
                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(edit.value);

                var rootGuidStr = edit.cms_root_component.ToString();
                var matchesRoot = doc.DocumentNode
                    .Descendants()
                    .Any(n =>
                        n.Attributes.Any(a =>
                            a.Name.Equals("data-cmsrootcompguid", StringComparison.OrdinalIgnoreCase)
                            && a.Value.Equals(rootGuidStr, StringComparison.OrdinalIgnoreCase)));

                if (validateDelete)
                {
                    var subGuidStr = edit.cmsedit_subcomponent.ToString();
                    var stillPresent = doc.DocumentNode
                        .Descendants()
                        .Any(n =>
                            n.Attributes.Any(a =>
                                a.Name.Equals("data-cmseditguid", StringComparison.OrdinalIgnoreCase)
                                && a.Value.Equals(subGuidStr, StringComparison.OrdinalIgnoreCase)));
                    if (stillPresent)
                        return new UpdateResults { result = DBActionResult.Failed, value_result = string.Empty };
                }
            }
            catch
            {
                // If parsing fails, treat as bad input
                return IUpdateResultCreator.Failed();
            }

            if (ct.IsCancellationRequested)
                return IUpdateResultCreator.Cancelled(string.Empty);

            using var trans = await _ctx.Database.BeginTransactionAsync(ct);
            try
            {
                var payload = IPayloadFactory.Create(edit.value);

                var ath = await _ctx.Authors.FirstOrDefaultAsync(ct);
                if (ath is not null)
                    payload.CreationAuthor = ath;

                _ctx.ComponentMarkups.Add(payload);

                authored.PayLoad = payload;
                authored.Published = DateTime.UtcNow;

                _ctx.AuthoredComponents.Update(authored);
                await _ctx.SaveChangesAsync(ct);

                if (ct.IsCancellationRequested)
                    return IUpdateResultCreator.Cancelled(string.Empty);

                await trans.CommitAsync(ct);
                return IUpdateResultCreator.Ok(edit.value);
            }
            catch
            {
                await trans.RollbackAsync(ct);
                return IUpdateResultCreator.Failed();
            }
            finally
            {
                trans.Dispose();
            }
        }
    }
    public interface IUpdateResultCreator
    {
        public static UpdateResults SlugNull()
            => new() { result = DBActionResult.Error };
        public static UpdateResults Ok(string value)
            => new() { result = DBActionResult.Succeded, value_result = value };
        public static UpdateResults Cancelled(string? value)
            => new() { result = DBActionResult.Cancelled, value_result = value ?? string.Empty };
        public static UpdateResults Failed()
            => new() { result = DBActionResult.Failed };
        public static UpdateResults Failed(string reason)
            => new() { result = DBActionResult.Failed, value_result = reason };
        public static UpdateResults InternalError(string? value)
            => new() {
                value_result = value ?? string.Empty, result = DBActionResult.Error };
        public static UpdateResults NotFound(string? value = null)
            => new() {
                result = DBActionResult.NotFound, value_result = value ?? string.Empty };
    }

    public static class DBExtensions
    {
        public static IQueryable<PublishedPageSlug> WithLoadedPage(this IQueryable<PublishedPageSlug> slug)
        {
            return slug.Include(x => x.Page);
        }

        public static IQueryable<PublishedPageSlug> WithLoadedPageVersion(this IQueryable<PublishedPageSlug> slug)
        {
            return slug
                .Include(x => x.PageVersion)
                .ThenInclude(x => x.PageTemplate)
                .Include(x => x.PageVersion)
                .ThenInclude(x => x.Components)
                .ThenInclude(x => x.PayLoad);
        }
        public static IQueryable<PublishedPageSlug> WithCompAndVersion(this IQueryable<PublishedPageSlug> slug)
        {
            return slug
                .Include(x => x.PageVersion)
                .ThenInclude(x => x.PageTemplate)
                .ThenInclude(x => x.PageComponents)
                .Include(x => x.PageVersion)
                .ThenInclude(x => x.Components)
                .ThenInclude(x => x.PayLoad);
        }

        // NEW: keep parity with existing call sites
        public static IQueryable<PublishedPageSlug> LoadedTemplate(this IQueryable<PublishedPageSlug> slug)
        {
            return slug
                .Include(x => x.PageVersion)
                    .ThenInclude(pv => pv.PageTemplate)
                        .ThenInclude(pt => pt.PageComponents)
                .AsSplitQuery();
        }
    }
    public interface IRequestContext
    {
        public static async Task<PublishedPageSlug?> GetLoadedSlug(ContentContext ctx, string path)
        {
            return await ctx
                .PublishedPages
                .Where(x => x.Slug.Slug == path)
                .WithLoadedPageVersion()
                .AsSplitQuery()
                .SingleOrDefaultAsync();
        }
    }
    public class PageEdit
    {
        [Required]
        [MaxLength(200)]
        [RegexStringValidator(@"^([a-z0-9\-\/]){1,200}$")]
        public string Url { get; set; }
        public ICollection<ContentEdit> ContentEdits { get; set; }
        public class ContentEdit
        {
            /// <summary>
            /// 0:Add, 1:Remove, 2, Edit
            /// </summary>
            [Required]
            [Range(0, 2)]

            [JsonConverter(typeof(JsonStringEnumConverter))]
            [AllowedValues(EditChoice.Add, EditChoice.Remove, EditChoice.Edit, EditChoice.Menu)]
            public EditChoice Choice { get; set; }

            /// <summary>
            /// 0:Text(html), 1:Header, 2:Image, 3:Link, 4:Css
            /// </summary>
            [Required]
            [Range(0, 4)]
            public ContentType ContentType { get; set; }

            /// <summary>
            /// For now simply dump the full HTML of the edited component
            ///     as a stringLiteral into this field, 
            ///     more complex behaviour is not implemented.
            /// Instructions Below are for future reference. Can be safely ignored.
            ///     
            /// Apporpriate Element Value,
            /// Text: Value is raw html of the entire element from the base of body
            /// Image: Relative link to the image, only support for uploaded Images
            /// Header: JsonObject appropriate for header
            /// Link: JsonObject appropriate for link
            /// Css: JsonObject appropriate for Css
            /// </summary>
            [Required]
            [MaxLength(10000)]
            public string Value { get; set; }

            /// <summary>
            /// RootElement Order
            /// </summary>
            [Required]
            public int PageOrder { get; set; }

            /// <summary>
            /// data-cmsGuid Guid Of RootElement
            /// </summary>
            [Required]
            public Guid? ComponentGuid { get; set; }
        }
    }

    public class SubComponentEdit
    {
        [Required]
        [MaxLength(200)]
        [RegexStringValidator(@"^([a-z0-9\-\/]){1,200}$")]
        public string url { get; set; }
        [Required]
        public Guid cms_root_component { get; set; }
        [Required]
        public Guid cmsedit_subcomponent { get; set; }
        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [AllowedValues(EditChoice.Add, EditChoice.Remove, EditChoice.Edit, EditChoice.Menu)]
        public EditChoice Choice { get; set; }
        [Required]
        [MaxLength(20000)]
        public string value { get; set; }
    }
    public class PageUpdates
    {
        public Guid element_guid { get; set; }
        public EditChoice edit_action { get; set; }
        public UpdateResults result { get; set; }
        public string exception_message { get; set; }
    }
    public class BadParam
    {
        public string Slug { get; set; }
        public string ExcMessage { get; set; }
    }
    public class UpdateResults
    {
        public DBActionResult result { get; set; }
        public string value_result { get; set; } = string.Empty;
    }
    public interface IPageContentManager
    {
        public Task<DBActionResult> CreatePageAsync(PageDetails page, CancellationToken token);
        public Task<DBActionResult> DeletePageAsync(PageDetails page, CancellationToken token);
        public Task<UpdateResults> UpdatePageAsync(PageEdit page, CancellationToken token);
    }
    public static class PageExentsions
    {
    }
    public interface ISlugFactory
    {
        public ContentDatabase.Model.PublishedPageSlug CreatePublished();
        public ContentDatabase.Model.PageSlug CreateSlug();
    }
    public interface IDefaultsRepo
    {
        public Task<ContentDatabase.Model.Page> GetDefaultPageAsync();
    }
    public interface IMenuCreator
    {
        public Task<Guid> GetGuid(ContentContext ctx);
    }
    public class MenuCreator(IServiceProvider prov, IConfiguration conf) : IMenuCreator
    {
        const string magic = "MagicGuid";
        const string filename = "runtimevars.json";
        private IServiceProvider _prov = prov;
        private IConfiguration _conf = conf;
        private Guid? magicGuid { get; set; } = null;
        public async Task<Guid> GetGuid(ContentContext ctx)
        {
            if (magicGuid is null) await FileCheck(ctx);

            if (magicGuid is null) await CreateMenu(ctx);

            if (magicGuid is null) throw new ArgumentException("No MagicGuid, no MagicNav");

            return magicGuid ?? new Guid();
        }

        private async Task FileCheck(ContentContext _ctx)
        {
            var res = _conf[magic];
            Guid id;
            if (res is null) return;
            if(Guid.TryParse(res, out id))
            {
                if ((await _ctx.AuthoredComponents.SingleOrDefaultAsync(x=> x.Id == id)) is null)
                {
                    magicGuid = null;
                    return;
                }
                magicGuid = id;
            }
        }

        private async Task CreateMenu(ContentContext ctx)
        {
            var _ctx = ctx;
            var autho = await _ctx.Authors.FirstAsync();
            var auth = IAuthoredComponentFactory.Create();
            var payl = IPayloadFactory.Create("<ul></ul>");
            payl.CreationAuthor = autho;
            _ctx.ComponentMarkups.Add(payl);
            await _ctx.SaveChangesAsync();
            auth.ComponentName = "main-header";
            auth.CreationAuthor = autho;
            auth.PayLoad = payl;
            auth.PageComponent = null;
            auth.PageVersion = null;
            _ctx.AuthoredComponents.Add(auth);
            await _ctx.SaveChangesAsync();
            magicGuid = auth.Id;
            using var stream = new FileStream(Path.Combine(AppContext.BaseDirectory, filename), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
            stream.SetLength(0);
                await JsonSerializer.SerializeAsync<MagicDump>(stream, new MagicDump() { MagicGuid = magicGuid }, new JsonSerializerOptions() { WriteIndented=true});
            await stream.FlushAsync();
            stream.Close();
            await stream.DisposeAsync();

            ((IConfigurationRoot)_prov.GetRequiredService<IConfiguration>()).Reload();
        }
        class MagicDump
        {
            public Guid? MagicGuid { get; set; }
        }
    }
    public interface IComponentsRepo
    {
        public AuthoredComponent GetComponent(Guid id);
        public DBActionResult UpdateComponent(Guid id, string payload);
        public Task<AuthoredComponent> GetComponentAsync(Guid id);
        public Task<DBActionResult> UpdateComponentAsync(Guid id, string payload);
    }
    public interface IMenuManger
    {
        public Task<NavModel> RetrieveMenuAsync();
        public Task<UpdateResults> EditMenuAsync(string content);
    }
    public class ComponentRepo(ContentContext ctx, ILogger<ComponentRepo> logger) : IComponentsRepo
    {
        private readonly ILogger<ComponentRepo> _logger = logger;
        private readonly ContentContext _ctx = ctx;
        public AuthoredComponent? GetComponent(Guid id)
        {
            return _ctx.AuthoredComponents
                .Include(x=> x.PayLoad)
                .SingleOrDefault(x => x.Id == id);
        }

        public async Task<AuthoredComponent?> GetComponentAsync(Guid id)
        {
            return await _ctx.AuthoredComponents
                .Include(x=> x.PayLoad)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public DBActionResult UpdateComponent(Guid id, string payload)
        {
            var auth = _ctx.Authors.First();
            var res = _ctx.AuthoredComponents.SingleOrDefault(x => x.Id == id);
            if (res is null) return DBActionResult.NotFound;

            var payl = IPayloadFactory.Create(payload);
            payl.CreationAuthor = auth;
            res.PayLoad = payl;
            try
            {
                _ctx.Update(res);
                _ctx.SaveChanges();
            }
            catch(Exception exc)
            {
                _logger.LogError(exc, "Threw when trying to save update of compoonent");
                return DBActionResult.Error;
            }
            return DBActionResult.Succeded;
        }

        public async Task<DBActionResult> UpdateComponentAsync(Guid id, string payload)
        {
            var auth = await _ctx.Authors.FirstAsync();
            var res = await _ctx.AuthoredComponents.SingleOrDefaultAsync(x => x.Id == id);
            if (res is null) return DBActionResult.NotFound;

            var payl = IPayloadFactory.Create(payload);
            payl.CreationAuthor = auth;
            res.PayLoad = payl;
            try
            {
                _ctx.Update(res);
                await _ctx.SaveChangesAsync();
            }
            catch(Exception exc)
            {
                _logger.LogError(exc, "Threw when trying to save update of compoonent");
                return DBActionResult.Error;
            }
            return DBActionResult.Succeded;

        }
    }
    public class MenuManager(IComponentsRepo repo, IMenuCreator mcr, ContentContext ctx) : IMenuManger
    {
        readonly IComponentsRepo _repo = repo;
        readonly IMenuCreator _mcr = mcr;
        readonly ContentContext _ctx = ctx;
        public async Task<UpdateResults> EditMenuAsync(string content)
        {
            var mg = await _mcr.GetGuid(_ctx);
            var ms = IAuthoredComponentFactory.Create();
            ms.PayLoad = IPayloadFactory.Create(content);
            try
            {
                var html = content;
                HtmlAgilityPack.HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(content);
                var start = doc.DocumentNode.SelectSingleNode("/");
                if(start.Attributes["data-cmsrootcompguid"] is not null)
                {
                    var node = doc.DocumentNode.SelectSingleNode("/*");
                    html = node.OuterHtml;
                }

                var r = await _repo.UpdateComponentAsync(mg, html);
                if (r.HasFlag(DBActionResult.Error)  ||
                    r.HasFlag(DBActionResult.Failed) ||
                    r.HasFlag(DBActionResult.NotFound))
                    return IUpdateResultCreator.Failed();

                return IUpdateResultCreator.Ok("Menu Updated");
                
            }
            catch(Exception exc)
            {
                return IUpdateResultCreator.Failed();
            }
        }

        public async Task<NavModel> RetrieveMenuAsync()
        {
            var mg = await _mcr.GetGuid(_ctx);
            AuthoredComponent comp;
            var mod = INavModelFactory.Create();
            try
            {
                comp = _repo.GetComponent(mg);
                mod.Content = comp.PayLoad.Markup;
                mod.Id = comp.Id;
            }
            catch(Exception exc)
            {
                //likely should be some kind of default. but i guess an empty model is a default of sorts
                return INavModelFactory.Create();
            }
            return mod;
        }
    }
    public interface INavModelFactory
    {
        public static NavModel Create()
        {
            return new NavModel
            {
                Content = string.Empty,
                Id = new Guid()
            };
        }
    }
    public class NavModel
    {
        public const string attrib = "data-cmsrootcompguid";
        public Guid Id { get; set; }
        public string Content { get; set; }
    }
    public class DefaultsCreator(ContentContext ctx) : IDefaultsRepo
    {
        private readonly ContentContext _ctx = ctx;

        public async Task<Page?> GetDefaultPageAsync()
        {
            return await _ctx.Pages.Where(x => x.PageName == "DefaultPage")
                .Include(x => x.PageVersions)!
                    .ThenInclude(x => x.Components)!
                .Include(x => x.PageVersions)!
                    .ThenInclude(x => x.PageTemplate)
                        .ThenInclude(x => x.PageComponents)
                .Include(x=> x.CreationAuthor)
                .OrderBy(x => x.Published)!
                .AsSplitQuery()
                .SingleOrDefaultAsync();
        }
    }
    public class SlugFactory : ISlugFactory
    {
        public SlugFactory()
        {
        }
        public PublishedPageSlug CreatePublished()
        {
            return new PublishedPageSlug()
            {
                Page = new(),
                PageVersion = new(),
                Slug = new(),
                PublishedAt = DateTime.UtcNow
            };
        }

        public PageSlug CreateSlug()
        {
            return new PageSlug()
            {
                Pages = [],
                Slug = string.Empty
            };
        }
    }
    public static class DbExtensionHelpers
    {
        public static IQueryable<PublishedPageSlug> PageSlugExistsExt(this IQueryable<ContentDatabase.Model.PublishedPageSlug> slug, string page)
        {
            return slug
                .Where(x => x.Slug.Slug == page);
        }
    }
    public class PageContentManager : IPageContentManager
    {
        private readonly ContentContext _ctx;
        private readonly ILogger<PageContentManager> _logger;
        private readonly ISlugFactory _slugfact;
        private readonly IDefaultsRepo _defaults;
        private readonly IDefaultPageCreator _creator;
        public PageContentManager(ContentContext ctx,
            ILogger<PageContentManager> logger,
            ISlugFactory slugfact, 
            IDefaultsRepo defaults,
            IDefaultPageCreator creator)
        {
            _slugfact = slugfact;
            _ctx = ctx;
            _logger = logger;
            _defaults = defaults;
            _creator = creator;
        }
        public async Task<DBActionResult> CreatePageAsync(PageDetails page, CancellationToken token)
        {
            var sl = page.Url.StartsWith('/') ? page.Url.Remove(0, 1) : page.Url;
            var exists = _ctx.PublishedPages.PageSlugExistsExt(sl);
            var now = DateTime.UtcNow;
            if((await exists.ToListAsync()).Count < 1)
            {
                var def = await _defaults.GetDefaultPageAsync();

                var published = _slugfact.CreatePublished();
                var pageSlug = _slugfact.CreateSlug();
                try
                {
                    pageSlug.Slug = sl;
                    def.Slug = pageSlug;
                    def.PageName = page.PageName ?? sl;

                    _ctx.PageSlugs.Add(pageSlug);
                    _ctx.Pages.Update(def);

                    await _ctx.SaveChangesAsync(token);

                    published.Slug = pageSlug;
                    published.Page = def;
                    published.PageVersion = def.PageVersions!.First();
                    await _ctx.SaveChangesAsync(token);

                    _ctx.PublishedPages.Add(published);

                    await _creator.CreateAsync(token);

                    await _ctx.SaveChangesAsync(token);
                    return DBActionResult.Succeded;
                }
                catch(Exception exc)
                {
                    _logger.LogError(exc, exc.Message);
                    return DBActionResult.Error;
                }
            }
            return DBActionResult.Failed;
        }

        public async Task<DBActionResult> DeletePageAsync(PageDetails page, CancellationToken token)
        {
            var items = await _ctx.PublishedPages.PageSlugExistsExt(page.Url).ToListAsync();
            if (items.Count < 1)
                return DBActionResult.Failed;
            try
            {
                _ctx.Remove(items.First());
                await _ctx.SaveChangesAsync(token);
            }
            catch(Exception exc)
            {
                _logger.LogError(exc, exc.Message);
                return DBActionResult.Error;
            }
            return DBActionResult.Succeded;
        }

        public async Task<UpdateResults> UpdatePageAsync(PageEdit page, [EnumeratorCancellation] CancellationToken token)
        {
            var sl = page.Url.StartsWith('/') ? page.Url.Remove(0, 1) : page.Url;
            var slug = await  _ctx.PublishedPages
                .PageSlugExistsExt(sl)
                .LoadedTemplate()
                .SingleOrDefaultAsync(token);

            if (slug is null)
                throw new ArgumentNullException("no slug was found for page!");

                List<UpdateResults> res = new();
                foreach (var item in page.ContentEdits)
                {
                    switch (item.Choice)
                    {
                        case EditChoice.Remove:
                            res.Add(await RemoveComponent(item, slug, token));
                            break;
                        case EditChoice.Add:
                            res.Add(await AddHtmlComponent(item, slug, token));
                            break;
                        case EditChoice.Edit:
                            res.Add(await EditComponent(item, slug, token));
                            break;
                    }
                }
            if (res.All(x => x.result == DBActionResult.Succeded))
                return new UpdateResults() { result = DBActionResult.Succeded, value_result = " All items updated accordingly" };
            return new UpdateResults() { value_result = res.Where(x => x.result == DBActionResult.Failed 
                                                        || x.result == DBActionResult.Error)
                                                       .Select(x => x.value_result)
                                                       .Aggregate((x, y) => x += y),
            result = DBActionResult.Failed};
        }
        private async Task<UpdateResults> RemoveComponent(
            PageEdit.ContentEdit delete,
            PublishedPageSlug slug,
            CancellationToken token)
        {
            if (delete.ComponentGuid is null)
                return new()
                {
                    result = DBActionResult.Error,
                    value_result = string.Empty
                };

            var comp = slug.PageVersion!
                .PageTemplate!
                .PageComponents!
                .SingleOrDefault(x => x.Id == delete.ComponentGuid);

                comp ??= await _ctx.PageComponents.SingleOrDefaultAsync(x=> x.Id == delete.ComponentGuid, token);
                

            if (comp is not null ||
                slug.PageVersion!
                    .PageTemplate!
                    .PageComponents!
                    .Remove(comp))
            {

                var acomp = await _ctx.AuthoredComponents
                .Where(x =>
                x.PageVersion.Id == slug.PageVersionId)
                .Where(x => x.PageComponent.Id == comp.Id)
                .Include(x => x.PayLoad)
                .SingleOrDefaultAsync(token);


                var pagecom = await _ctx.PageComponents
                    .Where(x => x.Id == comp.Id)
                    .SingleOrDefaultAsync(token);

                var markup = await _ctx.ComponentMarkups.Where(x => x.Id == acomp!.PayLoad.Id)
                    .SingleOrDefaultAsync(token);


                await _ctx.AuthoredComponents.Where(x => x.Id == acomp.Id).ExecuteDeleteAsync(token);
                await _ctx.ComponentMarkups.Where(x => x.Id == markup.Id).ExecuteDeleteAsync(token);
                await _ctx.PageComponents.Where(x => x.Id == comp.Id).ExecuteDeleteAsync(token);
            }   
            
            try
            {
                await _ctx.SaveChangesAsync(token);
                return new()
                {
                    result = DBActionResult.Succeded,
                    value_result = string.Empty
                };
            }
            catch(Exception exc)
            {
                _logger.LogError(exc, exc.Message);
                return new()
                {
                    result = DBActionResult.Failed,
                    value_result = string.Empty
                };
            }
        }
        private async Task<UpdateResults> EditComponent(
            PageEdit.ContentEdit edit,
            PublishedPageSlug slug,
            CancellationToken token
            )
        {
            if (edit.ComponentGuid is null || edit.Value is null)
                throw new ArgumentNullException($"Input Object did not provide a override value for the component");

            var comp = slug.PageVersion!
                .PageTemplate!
                .PageComponents!
                .SingleOrDefault(x=> x.Id == edit.ComponentGuid);

            if (comp is null)
                return new()
                {
                    result = DBActionResult.NotFound,
                    value_result = string.Empty
                };

            if (token.IsCancellationRequested)
                return new()
                {
                    result = DBActionResult.NotFound,
                    value_result = string.Empty
                };

            var page = await _ctx.Entry(slug)
                .Reference(x => x.Page)
                .Query()
                .Include(x => x.PageVersions!
                    .Where(x => x.Id == slug.PageVersionId))
                    .ThenInclude(x => x.Components)
                .AsSplitQuery()
                .SingleOrDefaultAsync(token);

            if (token.IsCancellationRequested)
                return new()
                {
                    result = DBActionResult.Cancelled,
                    value_result = string.Empty
                };

            var auth = page!.PageVersions!.First()
                .Components
                 .Where(x => x.PageComponent.Id == edit.ComponentGuid)
                 .SingleOrDefault();

            if (auth is null)
                return new()
                {
                    result = DBActionResult.Failed,
                    value_result = string.Empty
                };

            using var trans = await _ctx.Database.BeginTransactionAsync(token);
            try
            {

                auth.Published = DateTime.UtcNow;
                var markup = IPayloadFactory.Create(edit.Value);
                var ath = await _ctx.Authors.FirstOrDefaultAsync(token);

                markup.CreationAuthor = ath!;

                _ctx.ComponentMarkups.Add(markup);

                auth.PayLoad = markup;

                _ctx.Update(auth);
                await _ctx.SaveChangesAsync(token);

                if (token.IsCancellationRequested)
                    return new()
                    {
                        result = DBActionResult.Cancelled,
                        value_result = string.Empty
                    };

                await trans.CommitAsync(token);
                return new()
                {
                    result = DBActionResult.Succeded,
                    value_result = edit.Value
                };
                
            }catch(Exception exc)
            {
                _logger.LogError(exc, exc.Message);
                await trans.RollbackAsync(token);
                return new()
                {
                    result = DBActionResult.Failed,
                    value_result = string.Empty
                };
            }
            finally
            {
                trans.Dispose();
            }
        }
        private async Task<UpdateResults> AddHtmlComponent(PageEdit.ContentEdit add,
            PublishedPageSlug slug,
            CancellationToken token)
        {
            var comp = IPageComponentFactory.Create();
            var auth = await _ctx.Authors.FirstAsync();
            var last = slug.PageVersion!
                .PageTemplate!
                .PageComponents!
                .OrderBy(x => x.SelfPageOrder)
                .Last()
                .SelfPageOrder;

            var split = SplitTemplate(add.Value);

            if (split[0] == string.Empty || split[1] == string.Empty)
                throw new ArgumentException();

            using var trans = await _ctx.Database.BeginTransactionAsync();

            try
            {
                if (add.PageOrder != last)
                {
                    var alter = slug
                        .PageVersion!
                        .PageTemplate!
                        .PageComponents!
                        .SkipWhile(x => x.SelfPageOrder < add.PageOrder);
                    foreach(var i in alter)
                    {
                        i.SelfPageOrder++;
                    }
                    _ctx.UpdateRange(alter);
                    await _ctx.SaveChangesAsync(token);
                }
                comp.CreationAuthor = auth;
                comp.SelfPageOrder = add.PageOrder;
                comp.ComponentHtml = split[0];
                if (split[0].Contains("<div>"))
                    comp.ChildOffset = split[0].Length - "</div>".Length;

                else comp.ChildOffset = split[0].Length - "</header>".Length;


                    slug.PageVersion!
                        .PageTemplate!
                        .PageComponents!
                        .Add(comp);

                _ctx.Update(slug);

                await _ctx.SaveChangesAsync(token);

                var authored = IAuthoredComponentFactory.Create();
                var payload = IPayloadFactory.Create(split[1]);
                payload.CreationAuthor = auth;
                payload.Pages = [authored];
                authored.CreationAuthor = auth;
                authored.PageVersion = slug.PageVersion;
                authored.ComponentName = $"{auth.Name ?? "default"}.{authored.Version}.{comp.SelfPageOrder}";
                authored.PageComponent = comp;
                authored.Published = DateTime.UtcNow;
                authored.PayLoad = payload;

                   _ctx.Add(authored);
                await _ctx.SaveChangesAsync(token);

                comp.AuthoredComponent!.Add(authored);

                _ctx.Update(comp);
                
                var page = await _ctx.Pages.Where(x => x.Id == slug.PageId)
                    .Include(x => x.PageVersions!.Where(x => x.Id == slug.PageVersionId))
                    .AsSplitQuery()
                    .SingleOrDefaultAsync(token);

                page!.PageVersions!.First().Components.Add(authored);

                await _ctx.SaveChangesAsync(token);

                if (token.IsCancellationRequested)
                {
                    await trans.RollbackAsync(token);
                    return new()
                    {
                        result = DBActionResult.Cancelled,
                        value_result = string.Empty
                    };
                }

                await trans.CommitAsync(token);
                return new()
                {
                    result = DBActionResult.Succeded,
                    value_result = add.Value
                };
            }
            catch(Exception exc)
            {
                _logger.LogError(exc, exc.Message);
                return new()
                {
                    value_result = string.Empty,
                    result = DBActionResult.Failed
                };
            }
            finally
            {
                trans.Dispose();
            }
        }
        private Task<DBActionResult> AddComponent(
            PageEdit.ContentEdit add,
            ContentDatabase.Model.PublishedPageSlug slug,
            CancellationToken token)
        {
            switch (add.ContentType)
            {
                case ContentType.Text:
                    throw new NotImplementedException();
                case ContentType.Link:
                    throw new NotImplementedException();
                case ContentType.Image:
                    throw new NotImplementedException();
                case ContentType.Css:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentException();
            }
        }

        private async Task<DBActionResult> AddLinkComponent(
            PageEdit.ContentEdit add,
            PublishedPageSlug slug,
            CancellationToken token)
        {
            throw new NotImplementedException();
        }

        private string[] SplitTemplate(string Temp)
        {
            string[] splitHtml = new string[2];
            var html = new HtmlDocument();
            html.LoadHtml(Temp);

            var parent = html.DocumentNode.
                SelectSingleNode("/div | /header");

            splitHtml[1] = parent.InnerHtml;
            parent.RemoveAllChildren();
            splitHtml[0] = parent.OuterHtml;
            return splitHtml;
        }
        private  Task<UpdateResults> PageEditChoices(EditChoice choice,
            Func<Task<UpdateResults>> add,
            Func<Task<UpdateResults>> remove,
            Func<Task<UpdateResults>> edit)
        {
            switch (choice)
            {
                case EditChoice.Add:
                    return add();
                case EditChoice.Remove:
                    return remove();
                case EditChoice.Edit:
                    return edit();
                default:
                    throw new ArgumentException();
            }
        }
    }
    public interface IPayloadFactory
    {
        static ContentDatabase.Model.ComponentMarkup Create(string value)
        {
            var markup = IComponentMarkupFactory.Create(value);
            return new ComponentMarkup()
            {
                Generated = DateTime.UtcNow,
                Published = DateTime.UtcNow,
                Constructed = DateTime.UtcNow,
                CreationAuthor = new(),
                Markup = markup.Markup,
                Content = markup.Content,
            };
        }
    }
    public class CLRMerge
    {
        public string Content { get; set; }
        public string Markup { get; set; }
    }
    public interface IComponentMarkupFactory
    {
        static CLRMerge Create(string html)
        {
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);
            string copy = html;

            return new CLRMerge
            {
                Markup = html,
                Content = doc.Text
            };
        }
    }
    public interface IPageComponentFactory
    {
        static ContentDatabase.Model.PageComponent Create()
        {
            return new PageComponent
            {
                ComponentHtml = string.Empty,
                AuthoredComponent = [],
                PageTemplate = new(),
                CssHeaderTags = string.Empty,
                JsBodyTags = string.Empty,
                JsHeaderTags = string.Empty,
                Assets = [],
                OtherHeaders = string.Empty,
                Generated = DateTime.UtcNow,
                Constructed = DateTime.UtcNow,
                Published = DateTime.UtcNow,
                LastRevisionTime = DateTime.UtcNow
            };
        }
    }
    public interface IAuthoredComponentFactory
    {
        static ContentDatabase.Model.AuthoredComponent Create()
        {
            return new AuthoredComponent()
            {
                PageComponent = new(),
                ComponentName = string.Empty,
                PageVersion = new(),
                Version = 1,
                PayLoad = new(),
                Assets = [],
                Published = DateTime.UtcNow,
                CssHeaderTags = string.Empty,
                JsBodyTags =string.Empty,
                JsHeaderTags = string.Empty,
                CreationAuthor = new(),
                Constructed = DateTime.UtcNow,
                OtherHeaders = string.Empty,
                Generated = DateTime.UtcNow,
            };
        }
        static ContentDatabase.Model.AuthoredComponent Create(AuthoredComponent comp)
        {
             return new AuthoredComponent()
            {
                PageComponent = comp.PageComponent,
                ComponentName = comp.ComponentName,
                PageVersion = new(),
                Version = 1,
                PayLoad = new(),
                Assets = comp.Assets,
                Published = DateTime.UtcNow,
                CssHeaderTags = comp.CssHeaderTags,
                JsBodyTags = comp.JsBodyTags,
                JsHeaderTags = comp.JsHeaderTags,
                CreationAuthor =  comp.CreationAuthor,
                Constructed = DateTime.UtcNow,
                OtherHeaders = comp.OtherHeaders,
                Generated = DateTime.UtcNow,
            };

        }
    }

    public enum ContentType { Text, Header, Image, Link, Css }
    public enum EditChoice { Add, Remove, Edit, Menu }
    public enum LinkType { button, anchor  }

    public class LinkInsertion
    {
        [Required]
        [JsonRequired]
        [Range(0,1)]
        public LinkType type { get; set; }

        [JsonRequired]
        [Required]
        [Range(6,long.MaxValue)]
        public long tag_start_document_offset { get; set; }

        [Required]
        [JsonRequired]
        public string internal_text { get; set; }

        [Required]
        [JsonRequired]
        public Guid body_base_anchor_guid { get; set; }
    }
    public class PageDetails
    {
        [Required]
        [MaxLength(2000)]
        [RegexStringValidator(@"^([a-z0-9\-\/]){1,200}$")]
        public string Url { get; set; }
        [AllowNull]
        public string? PageName { get; set; }
        [AllowNull]
        public string? PageFocus { get; set; }
        [AllowNull]
        public string? IconUrl { get; set; }
        [AllowNull]
        public Guid? AuthorId { get; set; }
        string Value { get; set; }
    }
    public enum DBActionResult { Succeded, Failed, Error, NotFound, Cancelled }

    public interface ICounterApi
    {
        Task<int?> GetCountAsync(string key, CancellationToken ct = default);
    }

    public interface ICounterModelFactory
    {
        PageCounterModel Create(string key, int count);
    }

    public sealed class PageCounterModel
    {
        public string Key { get; init; } = string.Empty;
        public int Count { get; init; }
        public DateTime AsOfUtc { get; init; } = DateTime.UtcNow;
    }

    public sealed class CounterApi : ICounterApi
    {
        private readonly HttpClient _http;
        private readonly string _magic;

        public CounterApi(HttpClient http, IConfiguration cfg)
        {
            _http = http;
            _magic = cfg["Counter:MagicReadGuid"] ?? "8c6a2c4e-2e3a-4d77-9d7c-5f4a3b2c1d00";
        }

        public async Task<int?> GetCountAsync(string key, CancellationToken ct = default)
        {
            var url = $"/api/Counter?magic={Uri.EscapeDataString(_magic)}&key={Uri.EscapeDataString(key)}";
            using var res = await _http.GetAsync(url, ct);
            if (!res.IsSuccessStatusCode) return null;

            var payload = await res.Content.ReadFromJsonAsync<CounterResponse>(cancellationToken: ct);
            return payload?.count;
        }

        sealed class CounterResponse { public string? key { get; set; } public int count { get; set; } }
    }
}

public sealed class CounterModelFactory : ICounterModelFactory
{
    public PageCounterModel Create(string key, int count) =>
        new PageCounterModel { Key = key, Count = count, AsOfUtc = DateTime.UtcNow };
}

// DI extension
public static class CounterServices
{
    public static IServiceCollection AddPageCounters(this IServiceCollection services, IConfiguration cfg)
    {
        services.AddHttpClient<ICounterApi, CounterApi>(client =>
        {
            var baseUrl = cfg["Counter:BaseUrl"]; 
            if (!string.IsNullOrWhiteSpace(baseUrl))
                client.BaseAddress = new Uri(baseUrl, UriKind.Absolute);
        });

        services.AddSingleton<ICounterModelFactory, CounterModelFactory>();
        return services;
    }
}
