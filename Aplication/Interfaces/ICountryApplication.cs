using application.DTOs;

namespace application.Interfaces
{
    public interface ICountryApplication
    {
        Task<List<CountryListDto>> GetAllAsync();
    }
}
