using application.DTOs;

namespace presentations.Interfaces
{
    public interface ICitytPresentation : IBasePresentation
    {
        Task<List<CityListDto>> GetAllAsync();
    }
}
