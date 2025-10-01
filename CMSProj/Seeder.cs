using ContentDatabase.DeserliazationTypes;
using ContentDatabase.Model;
using System.Reflection;
using ContentDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Security.Cryptography.Xml;
using NuGet.Packaging;
using System.Runtime.InteropServices;
using CMSProj.Controllers;
using NuGet.Configuration;
namespace CMSProj
{

    public interface IAppSeeder
    {
        int Order { get; }
        Task SeedAsync(ContentContext db, CancellationToken ct);
    }

    public sealed class DefaultCmsSeeder : IAppSeeder
    {
        public int Order => 0;
        public async Task SeedAsync(ContentContext db, CancellationToken ct)
        {

            await db.Database.EnsureDeletedAsync();
            await db.Database.MigrateAsync();

            if (await db.Authors.AnyAsync(ct)) return;

            var now = DateTime.UtcNow;
            var systemAuthor = new Author
            {
                Name = "System",
                ContactEmail = "system@example.com",
                PageComponents = new List<PageComponent>(),
                PageTemplates = new List<PageTemplate>()
            };
            db.Authors.Add(systemAuthor);
            await db.SaveChangesAsync(ct);
            var authors = await db.Authors.ToListAsync();

            var png = new AssetFileType { Assets = new List<Assets>(), FileType = "image/png" };
            var jpg = new AssetFileType { Assets = new List<Assets>(), FileType = "image/jpeg" };
            var css = new AssetFileType { Assets = new List<Assets>(), FileType = "text/css" };
            var js = new AssetFileType { Assets = new List<Assets>(), FileType = "text/javascript" };
            db.AssetFileTypes.AddRange(png, jpg, css, js);

            var cdn = new AssetHostDomain { DomainName = "Local", DomainUrl = "https://localhost://", Assets = new List<Assets>() };
            var site = new AssetHostDomain { DomainName = "Site", DomainUrl = "https://www.example.com", Assets = new List<Assets>() };
            db.AssetHostDomains.AddRange(cdn, site);
            await db.SaveChangesAsync(ct);
            var fileTypes = await db.AssetFileTypes.ToListAsync();
            var domains = await db.AssetHostDomains.ToListAsync();

            var cssAsset = new Assets
            {
                Url = "https://cdn.example.com/styles/base.css",
                AssetName = "Base CSS",
                AssetDescription = "Base stylesheet",
                FileType = fileTypes.Where(x => x.FileType.Contains("css")).Single().FileType,
                AssetFileType = fileTypes.Where(x => x.FileType.Contains("css")).Single(),
                AssetDomain = domains.First(),
                Constructed = now,
                Generated = now,
                Published = now,
                CopyRight = "© Example",
                CopyRightDisclaimer = null,
                CreationAuthor = systemAuthor,
                LastRevisionTime = now,
                RevisionDiff = null,
                RevisionAuthor = new List<Author> { systemAuthor }
            };

            var jsAsset = new Assets
            {
                Url = "https://cdn.example.com/scripts/app.js",
                AssetName = "App JS",
                AssetDescription = "Frontend logic",
                FileType = fileTypes.Where(x=> x.FileType.Contains("javascript")).Single().FileType,
                AssetFileType = fileTypes.Where(x=> x.FileType.Contains("javascript")).Single(),
                AssetDomain = domains.First(),
                Constructed = now,
                Generated = now,
                Published = now,
                CopyRight = "© Example",
                CopyRightDisclaimer = null,
                CreationAuthor = systemAuthor,
                LastRevisionTime = now,
                RevisionDiff = null,
                RevisionAuthor = new List<Author> { systemAuthor }
            };

            db.Assets.AddRange(cssAsset, jsAsset);
            var baseTemplate = new PageTemplate
            {
                Version = 1,
                PageVersions = new List<PageVersion>(),
                Constructed = now,
                Generated = now,
                Published = now,
                CopyRight = "© Example",
                CopyRightDisclaimer = null,
                CreationAuthor = systemAuthor,
                LastRevisionTime = now,
                RevisionDiff = null,
                RevisionAuthor = new List<Author> { systemAuthor }
            };

            var defaultTemplate = new PageTemplate()
            {
                Version = 1,
                PageVersions = new List<PageVersion>(),
                Constructed = now,
                Generated = now,
                Published = now,
                CopyRight = "© Example",
                CopyRightDisclaimer = null,
                CreationAuthor = systemAuthor,
                LastRevisionTime = now,
                RevisionDiff = null,
                RevisionAuthor = new List<Author> { systemAuthor }
            };

            var notfoundTemplate = new PageTemplate
            {
                Version = 1,
                PageVersions = new List<PageVersion>(),
                Constructed = now,
                Generated = now,
                Published = now,
                CopyRight = "© Example",
                CopyRightDisclaimer = null,
                CreationAuthor = systemAuthor,
                LastRevisionTime = now,
                RevisionDiff = null,
                RevisionAuthor = new List<Author> { systemAuthor }
            };
            db.PageTemplates.AddRange(baseTemplate, notfoundTemplate, defaultTemplate);
            await db.SaveChangesAsync(ct);
            var templates = await db.PageTemplates.ToListAsync();
            var baseComponentHeader = new PageComponent
            {
                ComponentHtml = "<header class=\"navbar navbar-expand-lg navbar-dark bg-primary fixed-top\">\r\n \r\n</header>",
                PageTemplate = defaultTemplate, 
                ChildOffset = 79,
                SelfPageOrder = 0,
                Constructed = now,
                Generated = now,
                Published = now,
                CopyRightDisclaimer = null,
                CreationAuthor = authors.First(),
                LastRevisionTime = now,
                RevisionDiff = null,
                RevisionAuthor = new List<Author> { authors.First() }
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
                CreationAuthor = authors.First(),
                LastRevisionTime = now,
                RevisionDiff = null,
                RevisionAuthor = new List<Author> { authors.First() }
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
                CreationAuthor = authors.First(),
                LastRevisionTime = now,
                RevisionDiff = null,
                RevisionAuthor = new List<Author> { authors.First() }
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
                CreationAuthor = authors.First(),
                LastRevisionTime = now,
                RevisionDiff = null,
                RevisionAuthor = new List<Author> { authors.First() }
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
                CreationAuthor = authors.First(),
                LastRevisionTime = now,
                RevisionDiff = null,
                RevisionAuthor = new List<Author> { authors.First() }
            };


            var notfoundComponent = new PageComponent
            {
                ComponentHtml = "<header><h1></h1></header>",
                PageTemplate = notfoundTemplate,
                ChildOffset = 12,
                SelfPageOrder = 0,
                Constructed = now,
                Generated = now,
                Published = now,
                CopyRight = "© Example",
                CopyRightDisclaimer = null,
                CreationAuthor = authors.First(),
                LastRevisionTime = now,
                RevisionDiff = null,
                RevisionAuthor = new List<Author> { authors.First() }
            };
            var header = new PageComponent
            {
                ComponentHtml = "<header><h1></h1></header>",
                PageTemplate = baseTemplate,
                ChildOffset = 12,
                SelfPageOrder = 0,
                Constructed = now,
                Generated = now,
                Published = now,
                CopyRight = "© Example",
                CopyRightDisclaimer = null,
                CreationAuthor = authors.First(),
                LastRevisionTime = now,
                RevisionDiff = null,
                RevisionAuthor = new List<Author> { authors.First() }
            };

            var main = new PageComponent
            {
                ComponentHtml = "<main><section></section></main>",
                PageTemplate = baseTemplate,
                ChildOffset = 15,
                SelfPageOrder = 1,
                Constructed = now,
                Generated = now,
                Published = now,
                CopyRight = "© Example",
                CopyRightDisclaimer = null,
                CreationAuthor = authors.First(),
                LastRevisionTime = now,
                RevisionDiff = null,
                RevisionAuthor = new List<Author> { authors.First() }
            };

            var footer = new PageComponent
            {
                ComponentHtml = "<footer><small></small></footer>",
                PageTemplate = baseTemplate,
                ChildOffset = 15,
                SelfPageOrder = 2,
                Constructed = now,
                Generated = now,
                Published = now,
                CopyRight = "© Example",
                CopyRightDisclaimer = null,
                CreationAuthor = authors.First(),
                LastRevisionTime = now,
                RevisionDiff = null,
                RevisionAuthor = new List<Author> { authors.First() }
            };

            db.PageComponents.AddRange(header, main, footer,
                baseComponentHeader,
                baseComponentBod1,
                baseComponentBod2,
                baseComponentBod3,
                baseComponentBod4);

            await db.SaveChangesAsync(ct);
            var comps = await db.PageComponents.ToListAsync();


            var Slugs = new PageSlug() { Slug = "Home" };
            var notFound = new PageSlug() { Slug = "NotFound" };

            var def = new Page()
            {
                PageName = "DefaultPage",
                PageVersions = [],
                Constructed = now,
                Generated = now,
                Published = now,
                CopyRight = "© Example",
                CopyRightDisclaimer = null,
                CreationAuthor = systemAuthor,
                LastRevisionTime = now,
                RevisionDiff = null,
                RevisionAuthor = new List<Author> { systemAuthor }
            };
            var homePage = new Page
            {
                Slug = Slugs,
                PageName = "Home",
                PageVersions = new List<PageVersion>(),
                Constructed = now,
                Generated = now,
                Published = now,
                CopyRightDisclaimer = null,
                CreationAuthor = systemAuthor,
                LastRevisionTime = now,
                RevisionDiff = null,
                RevisionAuthor = new List<Author> { systemAuthor }
            };
            var notFoundPage = new Page
            {
                Slug = notFound,
                PageName = "Notfound",
                PageVersions = [],
                Constructed = now,
                Generated = now,
                Published =now,
                CopyRight = "© Example",
                CopyRightDisclaimer = null,
                CreationAuthor = systemAuthor,
                LastRevisionTime = now,
                RevisionDiff = null,
                RevisionAuthor = [systemAuthor],
            };
            db.Pages.AddRange(homePage, notFoundPage, def);
            await db.SaveChangesAsync(ct);
            var page = await db.Pages.ToListAsync();

            var defaultVersion = new PageVersion()
            {
                Version = 1,
                Components = new List<AuthoredComponent>(),
                PageTemplate = defaultTemplate,
                Page = def,
                Constructed = now,
                Generated = now,
                Published = now,
                CopyRight = "© Example",
                CopyRightDisclaimer = null,
                CreationAuthor = systemAuthor,
                LastRevisionTime = now,
                RevisionDiff = null,
                RevisionAuthor = new List<Author> { systemAuthor }
            };

            var v1 = new PageVersion
            {
                Version = 1,
                Components = new List<AuthoredComponent>(),
                PageTemplate = baseTemplate,
                Page = page.First(),
                Constructed = now,
                Generated = now,
                Published = now,
                CopyRight = "© Example",
                CopyRightDisclaimer = null,
                CreationAuthor = systemAuthor,
                LastRevisionTime = now,
                RevisionDiff = null,
                RevisionAuthor = new List<Author> { systemAuthor }
            };
            var notFv1 = new PageVersion
            {
                Version = 1,
                Components = new List<AuthoredComponent>(),
                PageTemplate = notfoundTemplate,
                Page = notFoundPage,
                Constructed = now,
                Generated = now,
                Published = now,
                CopyRight = "© Example",
                CopyRightDisclaimer = null,
                CreationAuthor = systemAuthor,
                LastRevisionTime = now,
                RevisionDiff = null,
                RevisionAuthor = new List<Author> { systemAuthor }
            };
            db.PageVersions.Add(v1);
            await db.SaveChangesAsync(ct);
            var versions = await db.PageVersions.ToListAsync();

            var notfContents = new CLRComponentContent()
            {
                Content = [
                    new() {Id = Guid.NewGuid(), Content = "404, Not Found"}
                    ]
            };
            var notFOffset = new List<CLRComponentMarkup.ContentOffset>()
            {
                new (){ ContentId = notfContents.Content[0].Id, Offset=4 }
            };
            var c1Contents = new CLRComponentContent()
            {
                Content = [
                    new () { Id = Guid.NewGuid(), Content="Welcome"},
                    new () { Id = Guid.NewGuid(), Content= "To the Site"}
                    ]
            };
            var c1Offsets = new List<CLRComponentMarkup.ContentOffset>
            {
                new CLRComponentMarkup.ContentOffset { ContentId = c1Contents.Content[0].Id, Offset = 4 },
                new CLRComponentMarkup.ContentOffset { ContentId = c1Contents.Content[1].Id, Offset = 9 }
            };
            var c1MarkupObj = new CLRComponentMarkup
            {
                Markup = "<h1></h1>\n",
                Content = c1Offsets
            };
            var notFoundMarkup = new CLRComponentMarkup
            {
                Markup = "<h1></h1>",
                Content = notFOffset
            };
            var notFoundPayload = new ComponentMarkup()
            {
                Markup = JsonSerializer.Serialize(notFoundMarkup),
                Content = JsonSerializer.Serialize(notfContents),
                Constructed = now,
                Generated = now,
                CreationAuthor = systemAuthor,
                LastRevisionTime = now,
                RevisionAuthor = [systemAuthor]
            };
            string accum = string.Empty;
            var headerPayload = new ComponentMarkup
            {
                Markup = c1MarkupObj.Markup,
                Content = c1Contents.Content.Aggregate(accum, (x, y)=> x +=y.Content),
                Constructed = now,
                Generated = now,
                CopyRight = "© Example",
                CopyRightDisclaimer = null,
                CreationAuthor = systemAuthor,
                LastRevisionTime = now,
                RevisionDiff = null,
                RevisionAuthor = new List<Author> { systemAuthor }
            };
            db.ComponentMarkups.AddRange(headerPayload, notFoundPayload);
            await db.SaveChangesAsync(ct);
            var notFauthoredComp = new AuthoredComponent()
            {
                PayLoad = notFoundPayload,
                ComponentName = "NotFoundComponent",
                PageComponent = notfoundComponent,
                Constructed = now,
                Generated = now,
                Published = now,
                CopyRightDisclaimer = null,
                CreationAuthor = systemAuthor,
                LastRevisionTime = now,
                RevisionDiff = null,
                RevisionAuthor = new List<Author> { systemAuthor }
            };
            var headerComponentInstance = new AuthoredComponent
            {
                PayLoad = headerPayload,
                ComponentName = "Header.Title.v1",
                CssHeaderTags = cssAsset.Url,
                JsHeaderTags = jsAsset.Url,
                JsBodyTags = null,
                PageVersion = v1,
                PageComponent = header,
                Constructed = now,
                Generated = now,
                Published = now,
                CopyRight = "© Example",
                CopyRightDisclaimer = null,
                CreationAuthor = systemAuthor,
                LastRevisionTime = now,
                RevisionDiff = null,
                RevisionAuthor = new List<Author> { systemAuthor }
            };
            var c1 = "<small>&copy; 2025 GenericCompany. All rights reserved.</small>";
            var c2 = "<h2 class=\"mb-4\">Services</h2>\r\n\r\n        <div data-role=\"collapsible-set\" data-theme=\"b\">\r\n            <div data-role=\"collapsible\">\r\n                <h3>Consulting</h3>\r\n                <p>Strategy, architecture reviews, and proof-of-concepts.</p>\r\n            </div>\r\n            <div data-role=\"collapsible\">\r\n                <h3>Development</h3>\r\n                <p>Full-stack web &amp; mobile apps built on modern tech.</p>\r\n            </div>\r\n            <div data-role=\"collapsible\">\r\n                <h3>Support</h3>\r\n                <p>24×7 monitoring, SLAs, and incident response.</p>\r\n            </div>\r\n        </div>";
            var c3 = "<h2>About Us</h2>\r\n        <p>\r\n            GenericCompany has delivered quality widgets and services since 2025.\r\n            Our team focuses on customer success, agile delivery, and coffee-fueled creativity.\r\n        </p>";
            var c4 = "<div class=\"container\">\r\n            <h1 class=\"display-4\">Welcome to GenericCompany</h1>\r\n            <p class=\"lead\">Your one-stop shop for utterly versatile solutions.</p>\r\n            <a href=\"#services\" class=\"btn btn-primary btn-lg\">What we do</a>\r\n        </div>";
            var c5 = "<a class=\"navbar-brand\" href=\"#\">GenericCompany</a>\r\n        <button class=\"navbar-toggler\" type=\"button\" data-toggle=\"collapse\"\r\n                data-target=\"#mainNav\" aria-controls=\"mainNav\"\r\n                aria-expanded=\"false\" aria-label=\"Toggle navigation\">\r\n            <span class=\"navbar-toggler-icon\"></span>\r\n        </button>\r\n\r\n        <nav id=\"mainNav\" class=\"collapse navbar-collapse\">\r\n            <ul class=\"navbar-nav ml-auto\">\r\n                <li class=\"nav-item\"><a class=\"nav-link\" href=\"#about\">About</a></li>\r\n                <li class=\"nav-item\"><a class=\"nav-link\" href=\"#services\">Services</a></li>\r\n                <li class=\"nav-item\"><a class=\"nav-link\" href=\"#contact\">Contact</a></li>\r\n            </ul>\r\n        </nav>";

            var cmpc1 = IComponentMarkupFactory.Create(c1);
            var cmpc2 = IComponentMarkupFactory.Create(c2);
            var cmpc3 = IComponentMarkupFactory.Create(c3);
            var cmpc4 = IComponentMarkupFactory.Create(c4);
            var cmpc5 = IComponentMarkupFactory.Create(c5);
            var defaultPayl1 = new ComponentMarkup
            {
                Constructed = now,
                Markup = cmpc1.Markup,
                Content = cmpc1.Content,
                Generated = now,
                CopyRight = "© Example",
                CopyRightDisclaimer = null,
                CreationAuthor = systemAuthor,
                LastRevisionTime = now,
                RevisionDiff = null,
                RevisionAuthor = new List<Author> { systemAuthor }

            };
            var defaultPayl2 = new ComponentMarkup
            {
                Constructed = now,
                Markup = cmpc2.Markup,
                Content = cmpc2.Content,
                Generated = now,
                CopyRight = "© Example",
                CopyRightDisclaimer = null,
                CreationAuthor = systemAuthor,
                LastRevisionTime = now,
                RevisionDiff = null,
                RevisionAuthor = new List<Author> { systemAuthor }

            };
            var defaultPayl3 = new ComponentMarkup
            {
                Constructed = now,
                Markup = cmpc3.Markup,
                Content = cmpc3.Content,
                Generated = now,
                CopyRight = "© Example",
                CopyRightDisclaimer = null,
                CreationAuthor = systemAuthor,
                LastRevisionTime = now,
                RevisionDiff = null,
                RevisionAuthor = new List<Author> { systemAuthor }

            };
            var defaultPayl4 = new ComponentMarkup
            {
                Constructed = now,
                Markup = cmpc4.Markup,
                Content = cmpc4.Content,
                Generated = now,
                CopyRight = "© Example",
                CopyRightDisclaimer = null,
                CreationAuthor = systemAuthor,
                LastRevisionTime = now,
                RevisionDiff = null,
                RevisionAuthor = new List<Author> { systemAuthor }

            };
            var defaultPayl5 = new ComponentMarkup
            {
                Constructed = now,
                Markup = cmpc5.Markup,
                Content = cmpc5.Content,
                Generated = now,
                CopyRight = "© Example",
                CopyRightDisclaimer = null,
                CreationAuthor = systemAuthor,
                LastRevisionTime = now,
                RevisionDiff = null,
                RevisionAuthor = new List<Author> { systemAuthor }
            };

            var c2Contents = new CLRComponentContent()
            {
                Content = [new () { Content = "<p>Getting started</p>" }]
            };
            var c2Offsets = new CLRComponentMarkup.ContentOffset()
            {
              ContentId = c2Contents.Content[0].Id, Offset = 9
            };
            var c2MarkupObj = new CLRComponentMarkup
            {
                Markup = "<section></section>",
                Content = [c2Offsets]
            };

            var mainPayload = new ComponentMarkup
            {
                Markup = "<p>Getting started</p>",
                Content = "Getting started",
                Constructed = now,
                Generated = now,
                Published = now,
                CopyRight = "© Example",
                CopyRightDisclaimer = null,
                CreationAuthor = systemAuthor,
                LastRevisionTime = now,
                RevisionDiff = null,
                RevisionAuthor = new List<Author> { systemAuthor }
            };
            db.ComponentMarkups.AddRange(mainPayload,
                defaultPayl1,
                defaultPayl2,
                defaultPayl3,
                defaultPayl4,
                defaultPayl5);
            await db.SaveChangesAsync(ct);

            var mainComponentInstance = new AuthoredComponent
            {
                PayLoad = mainPayload,
                ComponentName = "Main.Section.v1",
                CssHeaderTags = cssAsset.Url,
                JsHeaderTags = jsAsset.Url,
                JsBodyTags = null,
                PageVersion = v1,
                PageComponent = main,
                Constructed = now,
                Generated = now,
                Published = now,
                CopyRight = "© Example",
                CopyRightDisclaimer = null,
                CreationAuthor = systemAuthor,
                LastRevisionTime = now,
                RevisionDiff = null,
                RevisionAuthor = new List<Author> { systemAuthor }
            };

            var default1 = new AuthoredComponent
            {
                PayLoad = defaultPayl5,
                ComponentName = "Main.Section.v1",
                CssHeaderTags = cssAsset.Url,
                JsHeaderTags = jsAsset.Url,
                JsBodyTags = null,
                PageVersion = defaultVersion,
                PageComponent = baseComponentHeader,
                Constructed = now,
                Generated = now,
                Published = now,
                CopyRight = "© Example",
                CopyRightDisclaimer = null,
                CreationAuthor = systemAuthor,
                LastRevisionTime = now,
                RevisionDiff = null,
                RevisionAuthor = new List<Author> { systemAuthor }
            };
            var default2 = new AuthoredComponent
            {
                PayLoad = defaultPayl4,
                ComponentName = "Main.Section.v1",
                CssHeaderTags = cssAsset.Url,
                JsHeaderTags = jsAsset.Url,
                JsBodyTags = null,
                PageVersion = defaultVersion,
                PageComponent = baseComponentBod1,
                Constructed = now,
                Generated = now,
                Published = now,
                CopyRight = "© Example",
                CopyRightDisclaimer = null,
                CreationAuthor = systemAuthor,
                LastRevisionTime = now,
                RevisionDiff = null,
                RevisionAuthor = new List<Author> { systemAuthor }
            };
            var default3 = new AuthoredComponent
            {
                PayLoad = defaultPayl3,
                ComponentName = "Main.Section.v1",
                CssHeaderTags = cssAsset.Url,
                JsHeaderTags = jsAsset.Url,
                JsBodyTags = null,
                PageVersion = defaultVersion,
                PageComponent = baseComponentBod2,
                Constructed = now,
                Generated = now,
                Published = now,
                CopyRight = "© Example",
                CopyRightDisclaimer = null,
                CreationAuthor = systemAuthor,
                LastRevisionTime = now,
                RevisionDiff = null,
                RevisionAuthor = new List<Author> { systemAuthor }
            };
            var default4 = new AuthoredComponent
            {
                PayLoad = defaultPayl2,
                ComponentName = "Main.Section.v1",
                CssHeaderTags = cssAsset.Url,
                JsHeaderTags = jsAsset.Url,
                JsBodyTags = null,
                PageVersion = defaultVersion,
                PageComponent = baseComponentBod3,
                Constructed = now,
                Generated = now,
                Published = now,
                CopyRight = "© Example",
                CopyRightDisclaimer = null,
                CreationAuthor = systemAuthor,
                LastRevisionTime = now,
                RevisionDiff = null,
                RevisionAuthor = new List<Author> { systemAuthor }
            };
            var default5 = new AuthoredComponent
            {
                PayLoad = defaultPayl1,
                ComponentName = "Main.Section.v1",
                CssHeaderTags = cssAsset.Url,
                JsHeaderTags = jsAsset.Url,
                JsBodyTags = null,
                PageVersion = defaultVersion,
                PageComponent = baseComponentBod4,
                Constructed = now,
                Generated = now,
                Published = now,
                CopyRight = "© Example",
                CopyRightDisclaimer = null,
                CreationAuthor = systemAuthor,
                LastRevisionTime = now,
                RevisionDiff = null,
                RevisionAuthor = new List<Author> { systemAuthor }
            };


            versions.Where(x=> x.Page.PageName == "Home").First().Components.AddRange([mainComponentInstance, headerComponentInstance]);
            notFv1.Components.Add(notFauthoredComp);
            defaultVersion.Components.AddRange([default1, default2, default3, default4, default5]);
            db.Update(defaultVersion);
            def.PageVersions.Add(defaultVersion);
            db.Update(def);
            await db.SaveChangesAsync();
            db.PublishedPages.Add(new PublishedPageSlug() { Page = homePage, PageVersion = v1, PublishedAt = DateTime.Now, Slug = Slugs });
            db.PublishedPages.Add(new PublishedPageSlug { Page = notFoundPage, PageVersion = notFv1, PublishedAt = DateTime.Now, Slug = notFound });
            await db.SaveChangesAsync(ct);
        }
    }

    public sealed class RunSeedersHostedService : IHostedService
    {
        private readonly IServiceProvider _services;
        public RunSeedersHostedService(IServiceProvider services) => _services = services;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ContentContext>();
            var seeders = scope.ServiceProvider.GetRequiredService<IAppSeeder>();
            await seeders.SeedAsync(db, cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }

    public static class SeedingRegistrationExtensions
    {
        public static IServiceCollection AddCmsSeeders(this IServiceCollection services, params Assembly[] assemblies)
        {
            services.AddHostedService<RunSeedersHostedService>();
            services.AddSingleton<IAppSeeder, DefaultCmsSeeder>();
            return services;
        }
    }
}

