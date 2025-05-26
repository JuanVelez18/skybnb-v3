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
            IUnitOfWork unitOfWork )
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

            var newProperty = new Properties(
                propertiesCreationDto.Title,
                propertiesCreationDto.Description!,
                propertiesCreationDto.NumBathrooms,
                propertiesCreationDto.NumBedrooms,
                propertiesCreationDto.NumBeds,
                propertiesCreationDto.MaxGuests,
                propertiesCreationDto.BasePricePerNight,
                propertiesCreationDto.TypeId,
                propertiesCreationDto.HostId,
                propertiesCreationDto.AddressId
            );
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
    }
}