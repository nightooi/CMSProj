using CMSProj.DataLayer.PageServices.Components;
using CMSProj.SubSystems.Publishing.Extensions;

namespace CMSProj.SubSystems.Publishing
{
    public class ContentBuilder : IContentBuilder
    {
        IRenderContent _content;
        public ContentBuilder(IRenderContent rendercontent)
        {
            _content = rendercontent;
        }
        public IRenderContent BuildContent(PageAdapter adapter)
        {
            _content.Content.Add(ComponentKeys.Scaffolding, []);
            _content.Content[ComponentKeys.Scaffolding] = adapter.Scaffolding.ComponentHtml.Select(x=>x.html).ToList();
            _content.Content.Add(ComponentKeys.Header, [adapter.HeaderContents ?? ""]);
            _content.Content.Add(ComponentKeys.Js, [adapter.JsContents ?? ""]);
            _content.Content.Add(ComponentKeys.Component, []);
            _content.Content[ComponentKeys.Component] = adapter.PageContent.MergeContentWithComponent().ToList();
            _content.Content.Add(ComponentKeys.CompletePage, []);
            _content.Content[ComponentKeys.CompletePage] = adapter.Scaffolding.MergeComponentWithScaffold(adapter.PageContent).ToList();
            return _content;
        }
    }
}

