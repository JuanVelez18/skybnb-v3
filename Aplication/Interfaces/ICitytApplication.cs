using application.DTOs;

namespace application.Interfaces
{
    public interface ICitytApplication
    {
        Task<List<CityListDto>> GetAllAsync();
    }
}
