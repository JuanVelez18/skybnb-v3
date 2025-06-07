using application.DTOs;
using presentations.Interfaces;

namespace presentations.Implementations
{
    public class CityPresentation : BasePresentation, ICitytPresentation
    {
        public CityPresentation(Comunication comunication) : base(comunication)
        {
        }

        public async Task<List<CityListDto>> GetAllAsync()
        {
            var response = await _comunication.Execute<List<CityListDto>>("/cities");
            if (response.Ok)
            {
                return response.Data!;
            }
            else
            {
                throw new Exception($"Error: {response.Error!.Detail}");
            }
        }
    }
}
