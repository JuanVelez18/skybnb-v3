using application.DTOs;
using application.Interfaces;
using repository.Interfaces;

namespace application.Implementations
{
    public class CityApplication : ICitytApplication
    {
        private readonly IUnitOfWork _unitOfWork;

        public CityApplication(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<CityListDto>> GetAllAsync()
        {
            var cities = await _unitOfWork.Cities.GetAllAsync();
            return [.. cities.Select(c => new CityListDto
            {
                Id = c.Id,
                Name = c.Name
            })];
        }
    }
}
