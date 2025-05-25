using application.DTOs;
using presentations.Interfaces;

namespace presentations.Implementations
{
    public class CountryPresentation : BasePresentation, ICountryPresentation
    {
        public CountryPresentation(Comunication comunication) : base(comunication)
        {
        }

        public async Task<List<CountryListDto>> GetAllAsync()
        {
            var response = await _comunication.Execute<List<CountryListDto>>("/countries");
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
