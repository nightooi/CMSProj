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

            var png = new AssetFileType { Assets = new List<Assets>(), FileType = "image/png" };
            var jpg = new AssetFileType { Assets = new List<Assets>(), FileType = "image/jpeg" };
            var css = new AssetFileType { Assets = new List<Assets>(), FileType = "text/css" };
            var js = new AssetFileType { Assets = new List<Assets>(), FileType = "text/javascript" };
            db.AssetFileTypes.AddRange(png, jpg, css, js);

            var cdn = new AssetHostDomain { DomainName = "CDN", DomainUrl = "https://cdn.example.com", Assets = new List<Assets>() };
            var site = new AssetHostDomain { DomainName = "Site", DomainUrl = "https://www.example.com", Assets = new List<Assets>() };
            db.AssetHostDomains.AddRange(cdn, site);
            await db.SaveChangesAsync(ct);

            var Slugs = new PageSlug() { Slug = "/Home" };

            var cssAsset = new Assets
            {
                Url = "https://cdn.example.com/styles/base.css",
                AssetName = "Base CSS",
                AssetDescription = "Base stylesheet",
                FileType = css.FileType,
                AssetFileTypeId = css.Id,
                AssetDomain = cdn,
                Constructed = now,
                Generated = now,
                Published = now,
                AuthorId = systemAuthor.Id,
                CopyRight = "© Example",
                CopyRightDisclaimer = null,
                CreationAuthor = systemAuthor,
                LastRevisionTime = now,
                RevisionDiff = null,
                CreationAuthorId = systemAuthor.Id,
                RevisionAuthor = new List<Author> { systemAuthor }
            };

            var jsAsset = new Assets
            {
                Url = "https://cdn.example.com/scripts/app.js",
                AssetName = "App JS",
                AssetDescription = "Frontend logic",
                FileType = js.FileType,
                AssetFileTypeId = js.Id,
                AssetDomain = cdn,
                Constructed = now,
                Generated = now,
                Published = now,
                AuthorId = systemAuthor.Id,
                CopyRight = "© Example",
                CopyRightDisclaimer = null,
                CreationAuthor = systemAuthor,
                LastRevisionTime = now,
                RevisionDiff = null,
                CreationAuthorId = systemAuthor.Id,
                RevisionAuthor = new List<Author> { systemAuthor }
            };

            db.Assets.AddRange(cssAsset, jsAsset);

            var header = new PageComponent
            {
                ComponentHtml = "<header><h1></h1></header>",
                ChildOffset = 11,
                SelfPageOrder = 0,
                Constructed = now,
                Generated = now,
                Published = now,
                AuthorId = systemAuthor.Id,
                CopyRight = "© Example",
                CopyRightDisclaimer = null,
                CreationAuthor = systemAuthor,
                LastRevisionTime = now,
                RevisionDiff = null,
                CreationAuthorId = systemAuthor.Id,
                RevisionAuthor = new List<Author> { systemAuthor }
            };

            var main = new PageComponent
            {
                ComponentHtml = "<main><section></section></main>",
                ChildOffset = 14,
                SelfPageOrder = 1,
                Constructed = now,
                Generated = now,
                Published = now,
                AuthorId = systemAuthor.Id,
                CopyRight = "© Example",
                CopyRightDisclaimer = null,
                CreationAuthor = systemAuthor,
                LastRevisionTime = now,
                RevisionDiff = null,
                CreationAuthorId = systemAuthor.Id,
                RevisionAuthor = new List<Author> { systemAuthor }
            };

            var footer = new PageComponent
            {
                ComponentHtml = "<footer><small></small></footer>",
                ChildOffset = 14,
                SelfPageOrder = 2,
                Constructed = now,
                Generated = now,
                Published = now,
                AuthorId = systemAuthor.Id,
                CopyRight = "© Example",
                CopyRightDisclaimer = null,
                CreationAuthor = systemAuthor,
                LastRevisionTime = now,
                RevisionDiff = null,
                CreationAuthorId = systemAuthor.Id,
                RevisionAuthor = new List<Author> { systemAuthor }
            };

            db.PageComponents.AddRange(header, main, footer);
            await db.SaveChangesAsync(ct);

            var baseTemplate = new PageTemplate
            {
                Version = 1,
                PageComponents = new List<PageComponent> { header, main, footer },
                Pages = new List<Page>(),
                Constructed = now,
                Generated = now,
                Published = now,
                AuthorId = systemAuthor.Id,
                CopyRight = "© Example",
                CopyRightDisclaimer = null,
                CreationAuthor = systemAuthor,
                LastRevisionTime = now,
                RevisionDiff = null,
                CreationAuthorId = systemAuthor.Id,
                RevisionAuthor = new List<Author> { systemAuthor }
            };
            db.PageTemplates.Add(baseTemplate);
            await db.SaveChangesAsync(ct);

            var homePage = new Page
            {
                Slug = Slugs,
                PageName = "Home",
                PageVersions = new List<PageVersion>(),
                Constructed = now,
                Generated = now,
                Published = now,
                AuthorId = systemAuthor.Id,
                CopyRight = "© Example",
                CopyRightDisclaimer = null,
                CreationAuthor = systemAuthor,
                LastRevisionTime = now,
                RevisionDiff = null,
                CreationAuthorId = systemAuthor.Id,
                RevisionAuthor = new List<Author> { systemAuthor }
            };
            db.Pages.Add(homePage);
            await db.SaveChangesAsync(ct);

            var v1 = new PageVersion
            {
                PageId = homePage.Id,
                Version = 1,
                Components = new List<AuthoredComponent>(),
                Constructed = now,
                Generated = now,
                Published = now,
                AuthorId = systemAuthor.Id,
                CopyRight = "© Example",
                CopyRightDisclaimer = null,
                CreationAuthor = systemAuthor,
                LastRevisionTime = now,
                RevisionDiff = null,
                CreationAuthorId = systemAuthor.Id,
                RevisionAuthor = new List<Author> { systemAuthor }
            };
            db.PageVersions.Add(v1);
            await db.SaveChangesAsync(ct);

            var c1Contents = new List<CLRComponentContent>
            {
                new CLRComponentContent { Id = Guid.NewGuid(), Content = "Welcome" },
                new CLRComponentContent { Id = Guid.NewGuid(), Content = "to the site" }
            };
            var c1Offsets = new List<ContentOffset>
            {
                new ContentOffset { ContentId = c1Contents[0].Id, Offset = 9 },
                new ContentOffset { ContentId = c1Contents[1].Id, Offset = 9 }
            };
            var c1MarkupObj = new CLRComponentMarkup
            {
                Id = Guid.NewGuid(),
                Markup = "<h1></h1>",
                Content = c1Offsets
            };

            var headerPayload = new ComponentMarkup
            {
                Markup = JsonSerializer.Serialize(c1MarkupObj),
                Content = JsonSerializer.Serialize(c1Contents),
                Constructed = now,
                Generated = now,
                Published = now,
                AuthorId = systemAuthor.Id,
                CopyRight = "© Example",
                CopyRightDisclaimer = null,
                CreationAuthor = systemAuthor,
                LastRevisionTime = now,
                RevisionDiff = null,
                CreationAuthorId = systemAuthor.Id,
                RevisionAuthor = new List<Author> { systemAuthor }
            };
            db.ComponentMarkups.Add(headerPayload);
            await db.SaveChangesAsync(ct);

            var headerComponentInstance = new AuthoredComponent
            {
                PageComponentId = header.Id,
                PageVersionId = v1.Id,
                PayLoad = headerPayload,
                ComponentName = "Header.Title.v1",
                CssUrl = cssAsset.Url,
                JsUrl = jsAsset.Url,
                HeaderJsUrl = null,
                PageVersion = v1,
                PageComponent = header,
                Constructed = now,
                Generated = now,
                Published = now,
                AuthorId = systemAuthor.Id,
                CopyRight = "© Example",
                CopyRightDisclaimer = null,
                CreationAuthor = systemAuthor,
                LastRevisionTime = now,
                RevisionDiff = null,
                CreationAuthorId = systemAuthor.Id,
                RevisionAuthor = new List<Author> { systemAuthor }
            };

            var c2Contents = new List<CLRComponentContent>
            {
                new CLRComponentContent { Id = Guid.NewGuid(), Content = "<p>Getting started</p>" }
            };
            var c2Offsets = new List<ContentOffset>
            {
                new ContentOffset { ContentId = c2Contents[0].Id, Offset = 8 }
            };
            var c2MarkupObj = new CLRComponentMarkup
            {
                Id = Guid.NewGuid(),
                Markup = "<section></section>",
                Content = c2Offsets
            };

            var mainPayload = new ComponentMarkup
            {
                Markup = JsonSerializer.Serialize(c2MarkupObj),
                Content = JsonSerializer.Serialize(c2Contents),
                Constructed = now,
                Generated = now,
                Published = now,
                AuthorId = systemAuthor.Id,
                CopyRight = "© Example",
                CopyRightDisclaimer = null,
                CreationAuthor = systemAuthor,
                LastRevisionTime = now,
                RevisionDiff = null,
                CreationAuthorId = systemAuthor.Id,
                RevisionAuthor = new List<Author> { systemAuthor }
            };
            db.ComponentMarkups.Add(mainPayload);
            await db.SaveChangesAsync(ct);

            var mainComponentInstance = new AuthoredComponent
            {
                PageComponentId = main.Id,
                PageVersionId = v1.Id,
                PayLoad = mainPayload,
                ComponentName = "Main.Section.v1",
                CssUrl = cssAsset.Url,
                JsUrl = jsAsset.Url,
                HeaderJsUrl = null,
                PageVersion = v1,
                PageComponent = main,
                Constructed = now,
                Generated = now,
                Published = now,
                AuthorId = systemAuthor.Id,
                CopyRight = "© Example",
                CopyRightDisclaimer = null,
                CreationAuthor = systemAuthor,
                LastRevisionTime = now,
                RevisionDiff = null,
                CreationAuthorId = systemAuthor.Id,
                RevisionAuthor = new List<Author> { systemAuthor }
            };

            db.AuthoredComponents.AddRange(headerComponentInstance, mainComponentInstance);
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
            await db.Database.MigrateAsync(cancellationToken);
            var seeders = scope.ServiceProvider.GetServices<IAppSeeder>().OrderBy(s => s.Order).ToArray();
            foreach (var seeder in seeders) await seeder.SeedAsync(db, cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }

    public static class SeedingRegistrationExtensions
    {
        public static IServiceCollection AddCmsSeeders(this IServiceCollection services, params Assembly[] assemblies)
        {
            if (assemblies == null || assemblies.Length == 0) assemblies = new[] { Assembly.GetExecutingAssembly() };
            services.AddHostedService<RunSeedersHostedService>();
            return services;
        }
    }
}

