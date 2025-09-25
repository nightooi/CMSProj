namespace CMSProj.DataLayer.PageServices.Components
{
    public abstract class ComponentInfo
    {
        public Guid Id { get; protected set; }
        public DateTime Published { get; protected set; }
    }
}
