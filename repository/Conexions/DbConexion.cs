using System.Linq.Expressions;
using domain.Entities;
using Microsoft.EntityFrameworkCore;
using repository.Configuration;

namespace repository.Conexions
{
    public class DbConexion(DbContextOptions<DbConexion> options) : DbContext(options)
    {
        public DbSet<Auditories> Auditories { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Permissions> Permissions { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Customers> Customers { get; set; }
        public DbSet<Guests> Guests { get; set; }
        public DbSet<Countries> Countries { get; set; }
        public DbSet<Cities> Cities { get; set; }
        public DbSet<Addresses> Addresses { get; set; }
        public DbSet<PropertyTypes> PropertyTypes { get; set; }
        public DbSet<Properties> Properties { get; set; }
        public DbSet<Bookings> Bookings { get; set; }
        public DbSet<Reviews> Reviews { get; set; }
        public DbSet<RefreshTokens> RefreshTokens { get; set; }
        public DbSet<PropertyAssets> PropertyAssets { get; set; }

        private static LambdaExpression CreateSoftDeletedFilter(Type type)
        {
            var parameter = Expression.Parameter(type, "e");
            var propertyAccess = Expression.Property(Expression.Convert(parameter, typeof(IDisabled)), nameof(IDisabled.IsActive));
            var constantTrue = Expression.Constant(true);
            var body = Expression.Equal(propertyAccess, constantTrue);
            return Expression.Lambda(body, parameter);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Users>()
                .UseTptMappingStrategy();

            modelBuilder
                .Entity<Customers>()
                .HasOne(c => c.Country)
                .WithMany(c => c.Customers)
                .HasForeignKey(c => c.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<Guests>()
                .HasOne(g => g.Country)
                .WithMany(c => c.Guests)
                .HasForeignKey(g => g.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<Guests>()
                .HasOne(g => g.City)
                .WithMany(c => c.Guests)
                .HasForeignKey(g => g.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<Guests>()
                .HasOne(g => g.Address)
                .WithOne(a => a.Guest)
                .HasForeignKey<Guests>(g => g.AddressId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<Properties>()
                .HasOne(p => p.Type)
                .WithMany(pt => pt.Properties)
                .HasForeignKey(p => p.TypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<Properties>()
                .HasOne(p => p.Country)
                .WithMany(c => c.Properties)
                .HasForeignKey(p => p.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<Properties>()
                .HasOne(p => p.City)
                .WithMany(c => c.Properties)
                .HasForeignKey(p => p.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<Properties>()
                .HasOne(p => p.Address)
                .WithOne(a => a.Property)
                .HasForeignKey<Properties>(p => p.AddressId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<Bookings>()
                .HasOne(b => b.Guest)
                .WithMany(g => g.Bookings)
                .HasForeignKey(b => b.GuestId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder
                .Entity<Bookings>()
                .HasOne(b => b.Property)
                .WithMany(p => p.Bookings)
                .HasForeignKey(b => b.PropertyId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder
                .Entity<Payments>()
                .HasOne(p => p.User)
                .WithMany(g => g.Payments)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder
                .Entity<Reviews>()
                .HasOne(r => r.Guest)
                .WithMany(g => g.ReviewsWritten)
                .HasForeignKey(r => r.GuestId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder
                .Entity<Reviews>()
                .HasOne(r => r.Property)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.PropertyId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Roles>().HasData(
                InitialData.AdminRole,
                InitialData.HostRole,
                InitialData.GuestRole
            );

            modelBuilder.Entity<Permissions>().HasData(InitialData.GetAllPermissions());

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

                        var rolePermissions = InitialData.GetRolePermissionMappings();
                        var initialData = new List<object>();

                        foreach (var roleId in rolePermissions.Keys)
                        {
                            foreach (var permissionId in rolePermissions[roleId])
                            {
                                initialData.Add(new { RolesId = roleId, PermissionsId = permissionId });
                            }
                        }

                        j.HasData(initialData);
                    }
                );

            base.OnModelCreating(modelBuilder);

            // Apply soft delete filter to all entities implementing ISoftDeletable
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(IDisabled).IsAssignableFrom(entityType.ClrType) && entityType.BaseType == null)
                {
                    var filter = CreateSoftDeletedFilter(entityType.ClrType);
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(filter);
                }
            }
        }
    }
}
