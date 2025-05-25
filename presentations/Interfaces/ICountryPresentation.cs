using application.DTOs;

namespace presentations.Interfaces
{
    public interface ICountryPresentation : IBasePresentation
    {
        Task<List<CountryListDto>> GetAllAsync();
    }
}
