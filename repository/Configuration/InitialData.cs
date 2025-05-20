using domain.Entities;

namespace repository.Configuration
{
    public class InitialData
    {
        // ROLES
        public static readonly Roles AdminRole = new ("Admin", "Represent the system administrator") { Id = 1 };
        public static readonly Roles HostRole = new ("Host", "Represent the property owner") { Id = 2 };
        public static readonly Roles GuestRole = new ("Guest", "Represent the property guest") { Id = 3 };

        // PERMISSIONS
        // Country permissions
        public static readonly Permissions CreateCountryPermission = new ("create:country", "Permission to create a country") { Id = 1 };
        public static readonly Permissions ReadCountryPermission = new ("read:country", "Permission to read a country") { Id = 2 };
        public static readonly Permissions UpdateCountryPermission = new ("update:country", "Permission to update a country") { Id = 3 };

        // City permissions
        public static readonly Permissions CreateCityPermission = new("create:city", "Permission to create a city") { Id = 5 };
        public static readonly Permissions ReadCityPermission = new("read:city", "Permission to read a city") { Id = 6 };
        public static readonly Permissions UpdateCityPermission = new("update:city", "Permission to update a city") { Id = 7 };

        // Property type permissions
        public static readonly Permissions CreatePropertyTypePermission = new ("create:propertyType", "Permission to create a property type") { Id = 9 };
        public static readonly Permissions ReadPropertyTypePermission = new ("read:propertyType", "Permission to read a property type") { Id = 10 };
        public static readonly Permissions UpdatePropertyTypePermission = new ("update:propertyType", "Permission to update a property type") { Id = 11 };

        // Address permissions
        public static readonly Permissions CreateAddressPermission = new ("create:address", "Permission to create an address") { Id = 13 };
        public static readonly Permissions ReadAddressPermission = new ("read:address", "Permission to read an address") { Id = 14 };
        public static readonly Permissions UpdateAddressPermission = new ("update:address", "Permission to update an address") { Id = 15 };
        public static readonly Permissions DeleteAddressPermission = new ("delete:address", "Permission to delete an address") { Id = 16 };

        // Property permissions
        public static readonly Permissions CreatePropertyPermission = new ("create:property", "Permission to create a property") { Id = 17 };
        public static readonly Permissions ReadPropertyPermission = new ("read:property", "Permission to read a property") { Id = 18 };
        public static readonly Permissions UpdatePropertyPermission = new ("update:property", "Permission to update a property") { Id = 19 };
        public static readonly Permissions DeletePropertyPermission = new ("delete:property", "Permission to delete a property") { Id = 20 };

        // Booking permissions
        public static readonly Permissions CreateBookingPermission = new ("create:booking", "Permission to create a booking") { Id = 21 };
        public static readonly Permissions ReadBookingPermission = new ("read:booking", "Permission to read a booking") { Id = 22 };
        public static readonly Permissions UpdateBookingPermission = new ("update:booking", "Permission to update a booking") { Id = 23 };
        public static readonly Permissions DeleteBookingPermission = new ("delete:booking", "Permission to delete a booking") { Id = 24 };

        // Review permissions
        public static readonly Permissions CreateReviewPermission = new ("create:review", "Permission to create a review") { Id = 25 };
        public static readonly Permissions ReadReviewPermission = new ("read:review", "Permission to read a review") { Id = 26 };
        public static readonly Permissions UpdateReviewPermission = new ("update:review", "Permission to update a review") { Id = 27 };
        public static readonly Permissions DeleteReviewPermission = new ("delete:review", "Permission to delete a review") { Id = 28 };

        public static List<Permissions> GetAllPermissions()
        {
            return [
                CreateCountryPermission, ReadCountryPermission, UpdateCountryPermission,
                CreateCityPermission, ReadCityPermission, UpdateCityPermission,
                CreatePropertyTypePermission, ReadPropertyTypePermission, UpdatePropertyTypePermission,
                CreateAddressPermission, ReadAddressPermission, UpdateAddressPermission, DeleteAddressPermission,
                CreatePropertyPermission, ReadPropertyPermission, UpdatePropertyPermission, DeletePropertyPermission,
                CreateBookingPermission, ReadBookingPermission, UpdateBookingPermission, DeleteBookingPermission,
                CreateReviewPermission, ReadReviewPermission, UpdateReviewPermission, DeleteReviewPermission
            ];
        }

        public static Dictionary<int, List<int>> GetRolePermissionMappings()
        {
            return new Dictionary<int, List<int>>
            {
                [AdminRole.Id] = [
                    CreateCountryPermission.Id, ReadCountryPermission.Id, UpdateCountryPermission.Id,
                    CreateCityPermission.Id, ReadCityPermission.Id, UpdateCityPermission.Id,
                    CreatePropertyTypePermission.Id, ReadPropertyTypePermission.Id, UpdatePropertyTypePermission.Id,
                    UpdateAddressPermission.Id,
                    ReadPropertyPermission.Id, UpdatePropertyPermission.Id,
                    CreateBookingPermission.Id,
                    DeleteReviewPermission.Id
                ],
                [HostRole.Id] = [
                    CreatePropertyPermission.Id, ReadPropertyPermission.Id, UpdatePropertyPermission.Id, DeletePropertyPermission.Id,
                    ReadPropertyTypePermission.Id,
                    ReadBookingPermission.Id, UpdateBookingPermission.Id,
                    ReadReviewPermission.Id
                ],
                [GuestRole.Id] = [
                    ReadPropertyPermission.Id,
                    ReadPropertyTypePermission.Id,
                    CreateBookingPermission.Id, ReadBookingPermission.Id, UpdateBookingPermission.Id,
                    CreateReviewPermission.Id, ReadReviewPermission.Id
                ]
            };
        }
    }
}
