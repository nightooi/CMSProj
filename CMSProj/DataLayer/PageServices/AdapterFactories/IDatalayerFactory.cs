namespace CMSProj.DataLayer.PageServices.AdapterFactories
{
    public interface IDatalayerFactory<T, U>
    {
        public T Create(U model);
    }
}
