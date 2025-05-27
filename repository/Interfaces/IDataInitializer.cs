namespace repository.Interfaces
{
    public interface IDataInitializer
    {
        Task InitializeAsync();
        Task InitializeCountriesAsync();
        Task InitializeCitiesAsync();
        Task InitializePropertyTypesAsync();
    }
}
