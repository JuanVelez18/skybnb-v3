using application.DTOs;

namespace application.Interfaces
{
    public interface IPropertyTypesApplication
    {
        Task<List<PropertyTypesListDto>> GetAllAsync();
    }
}
