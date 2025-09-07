namespace ContentDatabase.Model
{
    public interface IComponentPeripheral
    {
        //Raw Html CSS to be included in the header
        public string? CssHeaderTags { get; set; }
        //Raw Html, Tag or populated script tag.
        public string? JsHeaderTags { get; set; }
        //Jstag or populated script tag.
        public string? JsBodyTags { get; set; }
        //UserAgentspecific unverifiableHeaders 
        public string? OtherHeaders { get; set; }
        public ICollection<Assets> Assets { get; set; }
    }
}
