using application.DTOs;

namespace application.Interfaces
{
    public interface IPropertiesApplication
    {
        Task CreateProperties(PropertiesCreationDto propertiesCreationDto, Guid hostId);
        Task<List<PropertyTypeDto>> GetAllPropertyTypesAsync();
    }
}
