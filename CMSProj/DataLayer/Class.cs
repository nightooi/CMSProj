using ContentDatabase;
using ContentDatabase.Model;

using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Client;
namespace CMSProj.DataLayer
{
    public interface IContentRetrievalService<T>
    {
        public T RetrieveContent(Guid guid);
        public Task<T> RetrieveContentAsync(Guid guid);

        public ICollection<T> RetrievePublishedOnContent(DateTime dateTime);
        public Task<ICollection<T>> RetrieveContentPublishedOnAsync(DateTime dateTime);

        public ICollection<T> RetrievePublishedBy(DateTime dateTime);
        public Task<ICollection<T>> RetrievePublishedByAsync(DateTime dateTime);

        public T RetrieveComponent(Guid guid);
        public Task<T> RetrieveComponentAsync(Guid guid);
    }

    public interface IPageOrchestratorService
    {
        //Idea is to start excavate Scaffold first and construct an empty page
        // and immiedately send it to the client
        public Page ConstructCompletePage(PageScaffold scaffoldedPage);
        public Task<Page> ConstructCompletePageAsync(PageScaffold scaffoldedPage);
        public Page ScaffoldPage(Guid guid);
        public Task<Page> ScaffoldPageAsync(Guid guid);
        public Page ScaffoldedPage(string url);
        public Task<Page> ScaffoldedPageAsync(string url);
        public Page ConstructedCompletePage(string url);
        public Task<Page> ConstructedCompletePageAsync(string url);
    }

    public interface IComponentOrchestrator
    {
        public ContentComponent 
    }
    
    public class PageOrchestrator : IPageOrchestratorService
    {
        IContentRetrievalService<PageScaffold> scaffoldRetrieval { get; }
        IContentRetrievalService<Asset> assetRetrieval { get; }
        IContentRetrievalService<Page> pageRetrieval { get; }
        IContentRetrievalService<ContentComponent> componentRetrieval { get; }

        public PageOrchestrator(
            IContentRetrievalService<PageScaffold> scaffoldService,
            IContentRetrievalService<Asset> assetService,
            IContentRetrievalService<Page> pageService,
            IContentRetrievalService<ContentComponent> componentService)
        {
            scaffoldRetrieval = scaffoldService;
            assetRetrieval = assetService
        }

        public Page ConstructCompletePage(Page scaffoldedPage)
        {
            throw new NotImplementedException();
        }

        public Task<Page> ConstructCompletePageAsync(Page scaffoldedPage)
        {
            throw new NotImplementedException();
        }

        public Page ConstructedCompletePage(string url)
        {
            throw new NotImplementedException();
        }

        public Task<Page> ConstructedCompletePageAsync(string url)
        {
            throw new NotImplementedException();
        }

        public Page ScaffoldedPage(string url)
        {
            throw new NotImplementedException();
        }

        public Task<Page> ScaffoldedPageAsync(string url)
        {
            throw new NotImplementedException();
        }

        public Page ScaffoldPage(Guid guid)
        {
            throw new NotImplementedException();
        }

        public Task<Page> ScaffoldPageAsync(Guid guid)
        {
            throw new NotImplementedException();
        }
    }

    public interface IDatalayerFactory<T, U>
    {
        public T Create(U model);
    }

    public class AssetFactory : IDatalayerFactory<CMSProj.DataLayer.Asset, ContentDatabase.Model.Assets>
    {
        public Asset Create(Assets model)
        {
            throw new NotImplementedException();
        }
    }

    public class ScaffoldedPageFactory : IDatalayerFactory<CMSProj.DataLayer.Page, ContentDatabase.Model.PageTemplate>
    {
        public Page Create(PageTemplate model)
        {
            throw new NotImplementedException();
        }
    }
    public class PageFactory : IDatalayerFactory<CMSProj.DataLayer.Page, ContentDatabase.Model.Page>
    {
        public Page Create(ContentDatabase.Model.Page model)
        {
            throw new NotImplementedException();
        }
    }

    public class ContentComponentFactory : IDatalayerFactory<CMSProj.DataLayer.ContentComponent, ContentDatabase.Model.AuthoredComponent>
    {
        public ContentComponent Create(AuthoredComponent model)
        {
            throw new NotImplementedException();
        }
    }
    public class ComponentRetrievalService : IContentRetrievalService<ContentComponent>
    {
        public ContentComponent RetrieveComponent(Guid guid)
        {
            throw new NotImplementedException();
        }

        public Task<ContentComponent> RetrieveComponentAsync(Guid guid)
        {
            throw new NotImplementedException();
        }

        public ContentComponent RetrieveContent(Guid guid)
        {
            throw new NotImplementedException();
        }

        public Task<ContentComponent> RetrieveContentAsync(Guid guid)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<ContentComponent>> RetrieveContentPublishedOnAsync(DateTime dateTime)
        {
            throw new NotImplementedException();
        }

        public ICollection<ContentComponent> RetrievePublishedBy(DateTime dateTime)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<ContentComponent>> RetrievePublishedByAsync(DateTime dateTime)
        {
            throw new NotImplementedException();
        }

        public ICollection<ContentComponent> RetrievePublishedOnContent(DateTime dateTime)
        {
            throw new NotImplementedException();
        }
    }
    public class ScaffoldingRetrievalService : IContentRetrievalService<PageScaffold>
    {
        public PageScaffold RetrieveComponent(Guid guid)
        {
            throw new NotImplementedException();
        }

        public Task<PageScaffold> RetrieveComponentAsync(Guid guid)
        {
            throw new NotImplementedException();
        }

        public PageScaffold RetrieveContent(Guid guid)
        {
            throw new NotImplementedException();
        }

        public Task<PageScaffold> RetrieveContentAsync(Guid guid)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<PageScaffold>> RetrieveContentPublishedOnAsync(DateTime dateTime)
        {
            throw new NotImplementedException();
        }

        public ICollection<PageScaffold> RetrievePublishedBy(DateTime dateTime)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<PageScaffold>> RetrievePublishedByAsync(DateTime dateTime)
        {
            throw new NotImplementedException();
        }

        public ICollection<PageScaffold> RetrievePublishedOnContent(DateTime dateTime)
        {
            throw new NotImplementedException();
        }
    }
    public class AssetRetrievalService : IContentRetrievalService<Asset>
    {
        private ContentContext ctx { get; }
        public AssetRetrievalService(ContentContext context)
        {
            ctx = context;
        }
        public Asset RetrieveComponent(Guid guid)
        {
            throw new NotImplementedException();
        }

        public Task<Asset> RetrieveComponentAsync(Guid guid)
        {
            throw new NotImplementedException();
        }

        public Asset RetrieveContent(Guid guid)
        {
            throw new NotImplementedException();
        }

        public Task<Asset> RetrieveContentAsync(Guid guid)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Asset>> RetrieveContentPublishedOnAsync(DateTime dateTime)
        {
            throw new NotImplementedException();
        }

        public ICollection<Asset> RetrievePublishedBy(DateTime dateTime)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Asset>> RetrievePublishedByAsync(DateTime dateTime)
        {
            throw new NotImplementedException();
        }

        public ICollection<Asset> RetrievePublishedOnContent(DateTime dateTime)
        {
            throw new NotImplementedException();
        }
    }
    public abstract class CMSContent
    {
        public Guid Guid { get; }
        public string Html { get; private set; }
        public DateTime PublishDate { get; }
    }
    public class Asset : CMSContent
    {
        public string Url { get; }
        public string AssetFiletype { get; }
    }

    public class ContentComponent : CMSContent
    {
        public IReadOnlyCollection<Asset>? Assets { get; }
    }
    public class PageScaffold : CMSContent
    {
        public string Slug { get; }
        public string PageName { get; }
        public Asset PageIcon { get; }
    }
    public class Page : CMSContent
    {
        public PageScaffold Scaffolding { get; }
        public IReadOnlyCollection<ContentComponent> PageContent { get; }
    }
}
