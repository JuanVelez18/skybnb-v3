using application.DTOs;
using application.Interfaces;
using repository.Interfaces;

namespace application.Implementations
{
    public class PropertyTypesApplication : IPropertyTypesApplication
    {
            private readonly IUnitOfWork _unitOfWork;

            public PropertyTypesApplication(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }
            public async Task<List<PropertyTypesListDto>> GetAllAsync()
            {
                var types = await _unitOfWork.PropertyTypes.GetAllAsync();
                return [.. types.Select(t => new PropertyTypesListDto
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description
            })];
            }
        }
    }

