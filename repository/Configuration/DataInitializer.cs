using domain.Entities;
using Microsoft.EntityFrameworkCore;
using repository.Conexions;
using repository.Interfaces;

namespace repository.Configuration
{
    public class DataInitializer : IDataInitializer
    {
        private readonly DbConexion _conexion;

        public DataInitializer(DbConexion conexion)
        {
            _conexion = conexion;
        }

        public async Task InitializeAsync()
        {
            try
            {
                Console.WriteLine("Initializing database...");

                await InitializeCountriesAsync();
                await InitializeCitiesAsync();
                await InitializePropertyTypesAsync();

                Console.WriteLine("Database initialized successfully.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error initializing database: {ex.Message}");
            }
        }

        public async Task InitializeCountriesAsync()
        {
            bool countriesExist = await _conexion.Countries.AnyAsync();
            if (countriesExist) return;

            Console.WriteLine("Initializing countries...");

            var countries = new List<Countries>
            {
                new (name: "Colombia", isoCode: "CO", phoneCode:"57"),
                new (name: "Argentina", isoCode: "AR", phoneCode:"54"),
                new (name: "Bolivia", isoCode: "BO", phoneCode:"591"),
                new (name: "Brasil", isoCode: "BR", phoneCode:"55"),
                new (name: "Chile", isoCode: "CL", phoneCode:"56"),
                new (name: "Ecuador", isoCode: "EC", phoneCode:"593"),
                new (name: "Paraguay", isoCode: "PY", phoneCode:"595"),
                new (name: "Peru", isoCode: "PE", phoneCode:"51"),
                new (name: "Uruguay", isoCode: "UY", phoneCode:"598"),
                new (name: "Venezuela", isoCode: "VE", phoneCode:"58"),
                new (name: "Mexico", isoCode: "MX", phoneCode:"52"),
                new (name: "Estados Unidos", isoCode: "US", phoneCode:"1")
            };

            await _conexion.Countries.AddRangeAsync(countries);
            await _conexion.SaveChangesAsync();

            Console.WriteLine($"{countries.Count} countries initialized successfully.");
        }

        public async Task InitializeCitiesAsync()
        {
            bool citiesExist = await _conexion.Cities.AnyAsync();
            if (citiesExist) return;

            Console.WriteLine("Initializing cities...");
            var country = await _conexion.Countries.FirstOrDefaultAsync(c => c.IsoCode == "CO");
            if (country == null)
            {
                Console.WriteLine("Country with ISO code 'CO' not found. Cannot initialize cities.");
                return;
            }

            var cities = new List<Cities>
            {
                new (name: "Bogotá", countryId: country.Id, latitude: 4.6102m, longitude: -74.0825m),
                new (name: "Medellín", countryId: country.Id, latitude: 6.2442m, longitude: -75.5812m),
                new (name: "Cali", countryId: country.Id, latitude: 3.4516m, longitude: -76.5320m),
                new (name: "Barranquilla", countryId: country.Id, latitude: 10.9685m, longitude: -74.7813m),
                new (name: "Cartagena", countryId: country.Id, latitude: 10.3910m, longitude: -75.4794m)
            };

            await _conexion.Cities.AddRangeAsync(cities);
            await _conexion.SaveChangesAsync();

            Console.WriteLine($"{cities.Count} cities initialized successfully.");
        }

        public async Task InitializePropertyTypesAsync()
        {
            bool propertyTypesExist = await _conexion.PropertyTypes.AnyAsync();
            if (propertyTypesExist) return;

            Console.WriteLine("Initializing property types...");

            var propertyTypes = new List<PropertyTypes>
            {
                new (name: "Apartment", description: "A self-contained housing unit that occupies part of a building."),
                new (name: "House", description: "A standalone residential building."),
                new (name: "Condo", description: "A unit in a building that is owned individually but shares common areas."),
                new (name: "Cabin", description: "A small, simple house, often in a rural area."),
                new (name: "Loft", description: "A large, open space typically converted from an industrial building."),
                new (name: "Villa", description: "A large and luxurious country house."),
            };

            await _conexion.PropertyTypes.AddRangeAsync(propertyTypes);
            await _conexion.SaveChangesAsync();

            Console.WriteLine($"{propertyTypes.Count} property types initialized successfully.");
        }
    }
}
