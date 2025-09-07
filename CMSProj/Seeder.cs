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
            db.PageTemplates.AddRange(baseTemplate);
            await db.SaveChangesAsync(ct);
            var templates = await db.PageTemplates.ToListAsync();
            var notfoundComponent = new PageComponent
            {
                ComponentHtml = "<header><h1></h1></header>",
                PageTemplate = templates.First(),
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
                PageTemplate = templates.First(),
                ChildOffset = 11,
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
                PageTemplate = templates.First(),
                ChildOffset = 17,
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
                PageTemplate = templates.First(),
                ChildOffset = 14,
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

            db.PageComponents.AddRange(header, main, footer);
            await db.SaveChangesAsync(ct);
            var comps = await db.PageComponents.ToListAsync();


            var Slugs = new PageSlug() { Slug = "Home" };
            var notFound = new PageSlug() { Slug = "NotFound" };
            var homePage = new Page
            {
                Slug = Slugs,
                PageName = "Home",
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
            db.Pages.AddRange(homePage, notFoundPage);
            await db.SaveChangesAsync(ct);
            var page = await db.Pages.ToListAsync();

            var v1 = new PageVersion
            {
                Version = 1,
                Components = new List<AuthoredComponent>(),
                PageTemplate = templates.First(),
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
                PageTemplate = templates.First(),
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
            db.PageVersions.Add(v1);
            await db.SaveChangesAsync(ct);
            var versions = await db.PageVersions.ToListAsync();
        
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

            var headerPayload = new ComponentMarkup
            {
                Markup = JsonSerializer.Serialize(c1MarkupObj),
                Content = JsonSerializer.Serialize(c1Contents),
                Constructed = now,
                Generated = now,
                CopyRight = "© Example",
                CopyRightDisclaimer = null,
                CreationAuthor = systemAuthor,
                LastRevisionTime = now,
                RevisionDiff = null,
                RevisionAuthor = new List<Author> { systemAuthor }
            };
            db.ComponentMarkups.Add(headerPayload);
            await db.SaveChangesAsync(ct);

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
                Markup = JsonSerializer.Serialize(c2MarkupObj),
                Content = JsonSerializer.Serialize(c2Contents),
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
            db.ComponentMarkups.Add(mainPayload);
            await db.SaveChangesAsync(ct);

            var mainComponentInstance = new AuthoredComponent
            {
                PayLoad = mainPayload,
                ComponentName = "Main.Section.v1",
                CssHeaderTags = cssAsset.Url,
                JsHeaderTags = jsAsset.Url,
                JsBodyTags = null,
                PageVersion = v1,
                PageComponent = comps.Single(x => x.ComponentHtml.Contains("main")),
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

