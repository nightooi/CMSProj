using CMSProj.DataLayer;

namespace CMSProj.Controllers
{
    public class RenderContent : IRenderContent
    {
        private Dictionary<ComponentKeys, ICollection<string>> _content = new();
        public IDictionary<ComponentKeys, ICollection<string>> Content => _content;

        public string InjectBody()
        {
            return _content[ComponentKeys.CompletePage].Aggregate((x, y) => x += y);
        }

        public string InjectHeaders()
        {
            return _content[ComponentKeys.Header].Aggregate((x, y) => x += y);
        }

        public string InjectScripts()
        {
            return _content[ComponentKeys.Js].Aggregate((x, y) => x += y);
        }
    }
}

