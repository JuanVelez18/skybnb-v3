using domain.Entities;

namespace repository.Configuration
{
    public class InitialData
    {
        // ROLES
        public static readonly Roles AdminRole = new("Admin", "Represent the system administrator") { Id = 1 };
        public static readonly Roles HostRole = new("Host", "Represent the property owner") { Id = 2 };
        public static readonly Roles GuestRole = new("Guest", "Represent the property guest") { Id = 3 };

        // PERMISSIONS
        // Users permissions
        public static readonly Permissions CreateUserPermission = new("create:user", "Permission to create a user") { Id = 1 };
        public static readonly Permissions ReadUserPermission = new("read:user", "Permission to read a user") { Id = 2 };
        public static readonly Permissions UpdateUserPermission = new("update:user", "Permission to update a user") { Id = 3 };
        public static readonly Permissions DeactivateUserPermission = new("deactivate:user", "Permission to deactivate a user") { Id = 4 };
        public static readonly Permissions DeleteUserPermission = new("delete:user", "Permission to delete a user") { Id = 5 };

        // Country permissions
        public static readonly Permissions CreateCountryPermission = new("create:country", "Permission to create a country") { Id = 6 };
        public static readonly Permissions ReadCountryPermission = new("read:country", "Permission to read a country") { Id = 7 };
        public static readonly Permissions UpdateCountryPermission = new("update:country", "Permission to update a country") { Id = 8 };
        public static readonly Permissions DeactivateCountryPermission = new("deactivate:country", "Permission to deactivate a country") { Id = 9 };
        public static readonly Permissions DeleteCountryPermission = new("delete:country", "Permission to delete a country") { Id = 10 };

        // City permissions
        public static readonly Permissions CreateCityPermission = new("create:city", "Permission to create a city") { Id = 11 };
        public static readonly Permissions ReadCityPermission = new("read:city", "Permission to read a city") { Id = 12 };
        public static readonly Permissions UpdateCityPermission = new("update:city", "Permission to update a city") { Id = 13 };
        public static readonly Permissions DeactivateCityPermission = new("deactivate:city", "Permission to deactivate a city") { Id = 14 };
        public static readonly Permissions DeleteCityPermission = new("delete:city", "Permission to delete a city") { Id = 15 };

        // Property type permissions
        public static readonly Permissions CreatePropertyTypePermission = new("create:propertyType", "Permission to create a property type") { Id = 16 };
        public static readonly Permissions ReadPropertyTypePermission = new("read:propertyType", "Permission to read a property type") { Id = 17 };
        public static readonly Permissions UpdatePropertyTypePermission = new("update:propertyType", "Permission to update a property type") { Id = 18 };
        public static readonly Permissions DeactivatePropertyTypePermission = new("deactivate:propertyType", "Permission to deactivate a property type") { Id = 19 };
        public static readonly Permissions DeletePropertyTypePermission = new("delete:propertyType", "Permission to delete a property type") { Id = 20 };

        // Auditories permissions
        public static readonly Permissions ReadAuditoriesPermission = new("read:auditories", "Permission to read auditories") { Id = 21 };

        // Property permissions
        public static readonly Permissions CreatePropertyPermission = new("create:property", "Permission to create a property") { Id = 22 };
        public static readonly Permissions ReadPropertyPermission = new("read:property", "Permission to read a property") { Id = 23 };
        public static readonly Permissions UpdatePropertyPermission = new("update:property", "Permission to update a property") { Id = 24 };
        public static readonly Permissions DeactivatePropertyPermission = new("deactivate:property", "Permission to deactivate a property") { Id = 25 };
        public static readonly Permissions DeletePropertyPermission = new("delete:property", "Permission to delete a property") { Id = 26 };

        // Booking permissions
        public static readonly Permissions CreateBookingPermission = new("create:booking", "Permission to create a booking") { Id = 27 };
        public static readonly Permissions ReadBookingPermission = new("read:booking", "Permission to read a booking") { Id = 28 };
        public static readonly Permissions UpdateBookingPermission = new("update:booking", "Permission to update a booking") { Id = 29 };
        public static readonly Permissions DeleteBookingPermission = new("delete:booking", "Permission to delete a booking") { Id = 30 };

        // Review permissions
        public static readonly Permissions CreateReviewPermission = new("create:review", "Permission to create a review") { Id = 31 };
        public static readonly Permissions ReadReviewPermission = new("read:review", "Permission to read a review") { Id = 32 };
        public static readonly Permissions UpdateReviewPermission = new("update:review", "Permission to update a review") { Id = 33 };
        public static readonly Permissions DeleteReviewPermission = new("delete:review", "Permission to delete a review") { Id = 34 };

        // Payment permissions
        public static readonly Permissions CreatePaymentPermission = new("create:payment", "Permission to create a payment") { Id = 35 };
        public static readonly Permissions ReadPaymentPermission = new("read:payment", "Permission to read a payment") { Id = 36 };

        public static List<Permissions> GetAllPermissions()
        {
            return [
                CreateUserPermission, ReadUserPermission, UpdateUserPermission, DeactivateUserPermission, DeleteUserPermission,
                CreateCountryPermission, ReadCountryPermission, UpdateCountryPermission, DeactivateCountryPermission, DeleteCountryPermission,
                CreateCityPermission, ReadCityPermission, UpdateCityPermission, DeactivateCityPermission, DeleteCityPermission,
                CreatePropertyTypePermission, ReadPropertyTypePermission, UpdatePropertyTypePermission, DeactivatePropertyTypePermission, DeletePropertyTypePermission,
                ReadAuditoriesPermission,
                CreatePropertyPermission, ReadPropertyPermission, UpdatePropertyPermission, DeactivatePropertyPermission, DeletePropertyPermission,
                CreateBookingPermission, ReadBookingPermission, UpdateBookingPermission, DeleteBookingPermission,
                CreateReviewPermission, ReadReviewPermission, UpdateReviewPermission, DeleteReviewPermission,
                CreatePaymentPermission, ReadPaymentPermission
            ];
        }

        public static Dictionary<int, List<int>> GetRolePermissionMappings()
        {
            return new Dictionary<int, List<int>>
            {
                [AdminRole.Id] = [
                    CreateUserPermission.Id, ReadUserPermission.Id, UpdateUserPermission.Id, DeactivateUserPermission.Id, DeleteUserPermission.Id,
                    CreateCountryPermission.Id, ReadCountryPermission.Id, UpdateCountryPermission.Id, DeactivateCountryPermission.Id, DeleteCountryPermission.Id,
                    CreateCityPermission.Id, ReadCityPermission.Id, UpdateCityPermission.Id, DeactivateCityPermission.Id, DeleteCityPermission.Id,
                    CreatePropertyTypePermission.Id, ReadPropertyTypePermission.Id, UpdatePropertyTypePermission.Id, DeactivatePropertyTypePermission.Id, DeletePropertyTypePermission.Id,
                    ReadAuditoriesPermission.Id,
                    ReadPropertyPermission.Id, UpdatePropertyPermission.Id, DeactivatePropertyPermission.Id, DeletePropertyPermission.Id,
                    ReadBookingPermission.Id, UpdateBookingPermission.Id, DeleteBookingPermission.Id,
                    ReadReviewPermission.Id, DeleteReviewPermission.Id,
                    ReadPaymentPermission.Id
                ],
                [HostRole.Id] = [
                    ReadUserPermission.Id, UpdateUserPermission.Id, DeactivateUserPermission.Id, DeleteUserPermission.Id,
                    ReadCountryPermission.Id,
                    ReadCityPermission.Id,
                    ReadPropertyTypePermission.Id,
                    CreatePropertyPermission.Id, ReadPropertyPermission.Id, UpdatePropertyPermission.Id, DeactivatePropertyPermission.Id, DeletePropertyPermission.Id,
                    ReadBookingPermission.Id, UpdateBookingPermission.Id,
                    CreateReviewPermission.Id, ReadReviewPermission.Id
                ],
                [GuestRole.Id] = [
                    ReadUserPermission.Id, UpdateUserPermission.Id, DeactivateUserPermission.Id, DeleteUserPermission.Id,
                    ReadCountryPermission.Id,
                    ReadCityPermission.Id,
                    ReadPropertyTypePermission.Id,
                    ReadPropertyPermission.Id,
                    CreateBookingPermission.Id, ReadBookingPermission.Id,
                    CreateReviewPermission.Id, ReadReviewPermission.Id,
                    CreatePaymentPermission.Id, ReadPaymentPermission.Id
                ]
            };
        }
    }
}
