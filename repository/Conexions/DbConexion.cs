using domain.Core;
using domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace repository.Conexions
{
    public class DbConexion: DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Configuration.ConexionString);
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

        public DbSet<Auditories> Auditories { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Permissions> Permissions { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Guests> Guests { get; set; }
        public DbSet<Countries> Countries { get; set; }
        public DbSet<Cities> Cities { get; set; }
        public DbSet<Addresses> Addresses { get; set; }
        public DbSet<PropertyTypes> PropertyTypes { get; set; }
        public DbSet<Properties> Properties { get; set; }
        public DbSet<Bookings> Bookings { get; set; }
        public DbSet<Reviews> Reviews { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure on delete behavior for entities with circular references
            modelBuilder
                .Entity<Properties>()
                .HasOne(p => p.Host)
                .WithMany(u => u.HostedProperties)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder
                .Entity<Bookings>()
                .HasOne(b => b.Property)
                .WithMany(p => p.Bookings)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder
                .Entity<Reviews>()
                .HasOne(r => r.Guest)
                .WithMany(u => u.ReviewsWritten)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder
                .Entity<Reviews>()
                .HasOne(r => r.Property)
                .WithMany(p => p.Reviews)
                .OnDelete(DeleteBehavior.ClientCascade);

            // ROLES
            var adminRole = new Roles("Admin", "Represent the system administrator") { Id = 1 };
            var hostRole = new Roles("Host", "Represent the property owner") { Id = 2 };
            var guestRole = new Roles("Guest", "Represent the property guest") { Id = 3 };

            // PERMISSIONS
            // Country permissions
            var createCountryPermission = new Permissions("create:country", "Permission to create a country") { Id = 1 };
            var readCountryPermission = new Permissions("read:country", "Permission to read a country") { Id = 2 };
            var updateCountryPermission = new Permissions("update:country", "Permission to update a country") { Id = 3 };

            // City permissions
            var createCityPermission = new Permissions("create:city", "Permission to create a city") { Id = 5 };
            var readCityPermission = new Permissions("read:city", "Permission to read a city") { Id = 6 };
            var updateCityPermission = new Permissions("update:city", "Permission to update a city") { Id = 7 };

            // Property type permissions
            var createPropertyTypePermission = new Permissions("create:propertyType", "Permission to create a property type") { Id = 9 };
            var readPropertyTypePermission = new Permissions("read:propertyType", "Permission to read a property type") { Id = 10 };
            var updatePropertyTypePermission = new Permissions("update:propertyType", "Permission to update a property type") { Id = 11 };

            // Address permissions
            var createAddressPermission = new Permissions("create:address", "Permission to create an address") { Id = 13 };
            var readAddressPermission = new Permissions("read:address", "Permission to read an address") { Id = 14 };
            var updateAddressPermission = new Permissions("update:address", "Permission to update an address") { Id = 15 };
            var deleteAddressPermission = new Permissions("delete:address", "Permission to delete an address") { Id = 16 };

            // Property permissions
            var createPropertyPermission = new Permissions("create:property", "Permission to create a property") { Id = 17 };
            var readPropertyPermission = new Permissions("read:property", "Permission to read a property") { Id = 18 };
            var updatePropertyPermission = new Permissions("update:property", "Permission to update a property") { Id = 19 };
            var deletePropertyPermission = new Permissions("delete:property", "Permission to delete a property") { Id = 20 };

            // Booking permissions
            var createBookingPermission = new Permissions("create:booking", "Permission to create a booking") { Id = 21 };
            var readBookingPermission = new Permissions("read:booking", "Permission to read a booking") { Id = 22 };
            var updateBookingPermission = new Permissions("update:booking", "Permission to update a booking") { Id = 23 };
            var deleteBookingPermission = new Permissions("delete:booking", "Permission to delete a booking") { Id = 24 };

            // Review permissions
            var createReviewPermission = new Permissions("create:review", "Permission to create a review") { Id = 25 };
            var readReviewPermission = new Permissions("read:review", "Permission to read a review") { Id = 26 };
            var updateReviewPermission = new Permissions("update:review", "Permission to update a review") { Id = 27 };
            var deleteReviewPermission = new Permissions("delete:review", "Permission to delete a review") { Id = 28 };

            modelBuilder.Entity<Roles>().HasData(adminRole, hostRole, guestRole);

            modelBuilder.Entity<Permissions>().HasData(
                createCountryPermission, readCountryPermission, updateCountryPermission,
                createCityPermission, readCityPermission, updateCityPermission,
                createPropertyTypePermission, readPropertyTypePermission, updatePropertyTypePermission,
                createAddressPermission, readAddressPermission, updateAddressPermission, deleteAddressPermission,
                createPropertyPermission, readPropertyPermission, updatePropertyPermission, deletePropertyPermission,
                createBookingPermission, readBookingPermission, updateBookingPermission, deleteBookingPermission,
                createReviewPermission, readReviewPermission, updateReviewPermission, deleteReviewPermission
            );

            // Assign permissions to roles
            modelBuilder.Entity<Roles>()
                .HasMany(r => r.Permissions)
                .WithMany(p => p.Roles)
                .UsingEntity<Dictionary<string, object>>(
                    "RolesPermissions",
                    l => l.HasOne<Permissions>().WithMany().HasForeignKey("PermissionsId"),
                    r => r.HasOne<Roles>().WithMany().HasForeignKey("RolesId"),
                    j =>
                    {
                        j.HasKey("RolesId", "PermissionsId");
                        j.HasData(
                            // Admin permissions
                            new { RolesId = adminRole.Id, PermissionsId = createCountryPermission.Id },
                            new { RolesId = adminRole.Id, PermissionsId = readCountryPermission.Id },
                            new { RolesId = adminRole.Id, PermissionsId = updateCountryPermission.Id },
                            new { RolesId = adminRole.Id, PermissionsId = createCityPermission.Id },
                            new { RolesId = adminRole.Id, PermissionsId = readCityPermission.Id },
                            new { RolesId = adminRole.Id, PermissionsId = updateCityPermission.Id },
                            new { RolesId = adminRole.Id, PermissionsId = createPropertyTypePermission.Id },
                            new { RolesId = adminRole.Id, PermissionsId = readPropertyTypePermission.Id },
                            new { RolesId = adminRole.Id, PermissionsId = updatePropertyTypePermission.Id },
                            new { RolesId = adminRole.Id, PermissionsId = updateAddressPermission.Id },
                            new { RolesId = adminRole.Id, PermissionsId = readPropertyPermission.Id },
                            new { RolesId = adminRole.Id, PermissionsId = updatePropertyPermission.Id },
                            new { RolesId = adminRole.Id, PermissionsId = createBookingPermission.Id },
                            new { RolesId = adminRole.Id, PermissionsId = deleteReviewPermission.Id },

                            // Host permissions
                            new { RolesId = hostRole.Id, PermissionsId = createPropertyPermission.Id },
                            new { RolesId = hostRole.Id, PermissionsId = readPropertyPermission.Id },
                            new { RolesId = hostRole.Id, PermissionsId = updatePropertyPermission.Id },
                            new { RolesId = hostRole.Id, PermissionsId = deletePropertyPermission.Id },
                            new { RolesId = hostRole.Id, PermissionsId = updateBookingPermission.Id },
                            new { RolesId = hostRole.Id, PermissionsId = readBookingPermission.Id },

                            // Guest permissions
                            new { RolesId = guestRole.Id, PermissionsId = createBookingPermission.Id },
                            new { RolesId = guestRole.Id, PermissionsId = updateBookingPermission.Id },
                            new { RolesId = guestRole.Id, PermissionsId = readBookingPermission.Id },
                            new { RolesId = guestRole.Id, PermissionsId = createReviewPermission.Id },
                            new { RolesId = guestRole.Id, PermissionsId = readReviewPermission.Id }
                        );
                    }
                );
        }
    }
}
