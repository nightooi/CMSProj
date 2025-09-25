namespace CMSProj.DataLayer.PageServices.Components
{
    public interface IDeserializer<T>
    {
        T? Deserialize(string Origin);
    }
}
