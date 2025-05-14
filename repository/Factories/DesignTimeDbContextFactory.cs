using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using repository.Conexions;

namespace repository.Factories
{
    public class DesignTimeDbContextFactory: IDesignTimeDbContextFactory<DbConexion>
    {
        public DbConexion CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddUserSecrets<DesignTimeDbContextFactory>()
                .Build();

            var connectionString = configuration.GetConnectionString("ConexionString");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("I can't find the connection string \"ConexionString\" in User Secrets");
            }

            var optionsBuilder = new DbContextOptionsBuilder<DbConexion>();
            optionsBuilder.UseSqlServer(connectionString);

            return new DbConexion(optionsBuilder.Options);
        }
    }
}
