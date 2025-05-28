using application.Core;
using application.DTOs;
using application.Interfaces;
using domain.Entities;
using repository.Configuration;
using repository.Interfaces;
using System.Text.Json;

namespace application.Implementations
{
    public class PropertiesApplication : IPropertiesApplication
    {
        private readonly IUnitOfWork _unitOfWork;

        public PropertiesApplication(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateProperties(PropertiesCreationDto propertiesCreationDto, Guid hostId)

        {
            var user = await _unitOfWork.Users.GetByIdAsync(hostId);

            if (user == null)
            {
                throw new NotFoundApplicationException("User not found.");
            }

            if (!user.Roles.Any(r => r.Id == InitialData.HostRole.Id))
            {
                throw new UnauthorizedApplicationException("The user is not a host.");
            }

            var newAddress = new Addresses(
                street: propertiesCreationDto.Address!.Street,
                streetNumber: propertiesCreationDto.Address.StreetNumber!.Value,
                intersectionNumber: propertiesCreationDto.Address.IntersectionNumber!.Value,
                doorNumber: propertiesCreationDto.Address.DoorNumber!.Value,
                cityId: propertiesCreationDto.Address.CityId!.Value,
                complement: propertiesCreationDto.Address.Complement,
                latitude: propertiesCreationDto.Address.Latitude,
                longitude: propertiesCreationDto.Address.Longitude
            );
            await _unitOfWork.Addresses.AddAsync(newAddress);

            var hasImages = propertiesCreationDto.Multimedia!.Any(m => m.Type == "image");
            if (!hasImages)
            {
                throw new InvalidDataApplicationException("At least one image is required.");
            }

            List<PropertyAssets> newMultimedia = [];
            foreach (var asset in propertiesCreationDto.Multimedia!)
            {
                var newAsset = new PropertyAssets()
                {
                    Url = asset.Url,
                    Type = asset.Type,
                    Order = asset.Order!.Value
                };
                await _unitOfWork.PropertyAssets.AddAsync(newAsset);
                newMultimedia.Add(newAsset);
            }

            var newProperty = new Properties(
                propertiesCreationDto.Title,
                propertiesCreationDto.Description!,
                propertiesCreationDto.NumBathrooms!.Value,
                propertiesCreationDto.NumBedrooms!.Value,
                propertiesCreationDto.NumBeds!.Value,
                propertiesCreationDto.MaxGuests!.Value,
                propertiesCreationDto.BasePricePerNight,
                propertiesCreationDto.TypeId!.Value,
                propertiesCreationDto.HostId,
                newAddress.Id
            );
            newProperty.Host = user;
            newProperty.Address = newAddress;
            newProperty.Multimedia = newMultimedia;

            await _unitOfWork.Properties.AddAsync(newProperty);

            var auditory = new Auditories(
                userId: hostId,
                action: "Register Property",
                entity: "Property",
                entityId: newProperty.Id.ToString(),
                details: JsonSerializer.Serialize(newProperty, new JsonSerializerOptions
                {
                    ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
                }),
                timestamp: DateTime.UtcNow
            );

            await _unitOfWork.Auditories.AddAsync(auditory);
            await _unitOfWork.CommitAsync();
        }

        public async Task<List<PropertyTypeDto>> GetAllPropertyTypesAsync()
        {
            var propertyTypes = await _unitOfWork.PropertyTypes.GetAllAsync();

            return [.. propertyTypes.Select(pt => new PropertyTypeDto
            {
                Id = pt.Id,
                Name = pt.Name,
                Description = pt.Description
            })];
        }

        public async Task<PageDto<PropertySummaryDto>> GetPropertiesAsync(PaginationOptionsDto paginationDto, PropertyFiltersDto? filtersDto, Guid? userId)
        {
            var filters = filtersDto?.ToDomainPropertyFilters();
            if (filters != null && !filters.IsValid())
            {
                throw new InvalidDataApplicationException("Invalid filters provided.");
            }

            var pagination = paginationDto.ToDomainPaginationOptions();
            if (!pagination.IsValid())
            {
                throw new InvalidDataApplicationException("Invalid pagination options provided.");
            }

            var auditory = new Auditories(
                userId,
                action: "Get Properties",
                entity: "Property",
                entityId: null,
                details: filtersDto != null ? JsonSerializer.Serialize(filtersDto) : null,
                timestamp: DateTime.UtcNow
            );
            await _unitOfWork.Auditories.AddAsync(auditory);
            await _unitOfWork.CommitAsync();

            var propertiesPage = await _unitOfWork.Properties.GetPropertiesAsync(pagination, filters);

            return PageDto<PropertySummaryDto>.FromDomainPage(propertiesPage, PropertySummaryDto.FromDomainProperty);
        }
    }
}