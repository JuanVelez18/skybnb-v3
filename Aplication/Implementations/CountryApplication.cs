using application.DTOs;
using application.Interfaces;
using repository.Interfaces;

namespace application.Implementations
{
    public class CountryApplication : ICountryApplication
    {
        private readonly IUnitOfWork _unitOfWork;

        public CountryApplication(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<CountryListDto>> GetAllAsync()
        {
            var countries = await _unitOfWork.Countries.GetAllAsync();
            return [.. countries.Select(c => new CountryListDto
            {
                Id = c.Id,
                Name = c.Name,
                IsoCode = c.IsoCode
            })];
        }
    }
}
