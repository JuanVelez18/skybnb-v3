using domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using repository.Conexions;
using repository.Interfaces;

namespace repository.Configuration
{
    public class DataInitializer : IDataInitializer
    {
        private readonly DbConexion _conexion;
        private readonly IPasswordHasher<object> _passwordHasher;

        public DataInitializer(DbConexion conexion, IPasswordHasher<object> passwordHasher)
        {
            _conexion = conexion;
            _passwordHasher = passwordHasher;
        }

        public async Task InitializeAsync()
        {
            try
            {
                Console.WriteLine("Initializing database...");

                await InitializeCountriesAsync();
                await InitializeCitiesAsync();
                await InitializePropertyTypesAsync();
                await InitializeCustomersAsync();
                await InitializePropertiesAsync();

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

        public async Task InitializeCustomersAsync()
        {
            bool customersExist = await _conexion.Customers.AnyAsync();
            if (customersExist) return;

            var countries = await _conexion.Countries.ToListAsync();
            if (countries.Count == 0)
            {
                Console.WriteLine("No countries found. Cannot initialize customers.");
                return;
            }

            var cities = await _conexion.Cities.Take(10).ToListAsync();
            if (cities.Count == 0)
            {
                Console.WriteLine("No cities found. Cannot initialize customers.");
                return;
            }

            var hostRole = await _conexion.Roles.FirstOrDefaultAsync(r => r.Id == InitialData.HostRole.Id);
            var guestRole = await _conexion.Roles.FirstOrDefaultAsync(r => r.Id == InitialData.GuestRole.Id);

            Console.WriteLine("Initializing customers...");

            var hosts = new List<Customers>
            {
                new (dni: "111111111", firstName: "User", lastName: "Host 1", email: "host.test+1@yopmail.com", passwordHash: _passwordHasher.HashPassword(null, "Host1234*"), birthday: new DateOnly(1990, 1, 1), countryId: countries[0].Id, phone: "3001234567"),
                new (dni: "222222222", firstName: "User", lastName: "Host 2", email: "host.test+2@yopmail.com", passwordHash: _passwordHasher.HashPassword(null, "Host1234*"), birthday: new DateOnly(1990, 1, 1), countryId: countries[0].Id, phone: "3001234568"),
            };
            foreach (var host in hosts) host.Roles.Add(hostRole!);

            await _conexion.Customers.AddRangeAsync(hosts);

            var guests = new List<Customers>
            {
                new (dni: "333333333", firstName: "User", lastName: "Guest 1", email: "guest.test+1@yopmail.com", passwordHash: _passwordHasher.HashPassword(null, "Guest1234*"), birthday: new DateOnly(1995, 1, 1), countryId: countries[0].Id, phone: "3001234569"),
                new (dni: "444444444", firstName: "User", lastName: "Guest 2", email: "guest.test+2@yopmail.com", passwordHash: _passwordHasher.HashPassword(null, "Guest1234*"), birthday: new DateOnly(1995, 1, 1), countryId: countries[0].Id, phone: "3001234570"),
            };
            foreach (var guest in guests) guest.Roles.Add(guestRole!);

            await _conexion.Customers.AddRangeAsync(guests);

            var addresses = new List<Addresses>
            {
                new (street: "Main St", streetNumber: 123, intersectionNumber: 0, doorNumber: 1, complement: null, latitude: 4.6102m, longitude: -74.0825m),
                new (street: "Second St", streetNumber: 456, intersectionNumber: 0, doorNumber: 2, complement: null, latitude: 6.2442m, longitude: -75.5812m),
            };
            var guestProfiles = new List<Guests>
            {
                new (customerId: guests[0].Id, addressId: addresses[0].Id, CityId: cities[0].Id, CountryId: countries[0].Id),
                new (customerId: guests[1].Id, addressId: addresses[0].Id, CityId: cities[1].Id, CountryId: countries[0].Id),
            };

            guestProfiles[0].Customer = guests[0];
            guestProfiles[1].Customer = guests[1];
            guestProfiles[0].Address = addresses[0];
            guestProfiles[1].Address = addresses[1];
            guestProfiles[0].City = cities[0];
            guestProfiles[1].City = cities[1];
            guestProfiles[0].Country = countries[0];
            guestProfiles[1].Country = countries[0];

            await _conexion.Guests.AddRangeAsync(guestProfiles);

            await _conexion.SaveChangesAsync();
        }

        public async Task InitializePropertiesAsync()
        {
            bool propertiesExist = await _conexion.Properties.AnyAsync();
            if (propertiesExist) return;

            Console.WriteLine("Initializing properties...");

            var countries = await _conexion.Countries.ToListAsync();
            if (countries.Count == 0)
            {
                Console.WriteLine("No countries found. Cannot initialize properties.");
                return;
            }

            var cities = await _conexion.Cities.ToListAsync();
            if (cities.Count == 0)
            {
                Console.WriteLine("No cities found. Cannot initialize properties.");
                return;
            }

            var propertyTypes = await _conexion.PropertyTypes.ToListAsync();
            if (propertyTypes.Count == 0)
            {
                Console.WriteLine("No property types found. Cannot initialize properties.");
                return;
            }

            var hosts = await _conexion.Customers.Where(c => c.Roles.Any(r => r.Id == InitialData.HostRole.Id)).ToListAsync();
            if (hosts.Count == 0)
            {
                Console.WriteLine("No hosts found. Cannot initialize properties.");
                return;
            }

            List<Addresses> addresses = [
                new (street: "Calle", streetNumber: 101, intersectionNumber: 3, doorNumber: 1, complement: null, latitude: null, longitude: null),
                new (street: "Avenida", streetNumber: 202, intersectionNumber: 4, doorNumber: 2, complement: null, latitude: null, longitude: null),
                new (street: "Carrera", streetNumber: 303, intersectionNumber: 5, doorNumber: 3, complement: null, latitude: null, longitude: null),
                new (street: "Circular", streetNumber: 40, intersectionNumber: 6, doorNumber: 4, complement: null, latitude: null, longitude: null),
                new (street: "Transversal", streetNumber: 50, intersectionNumber: 7, doorNumber: 5, complement: null, latitude: null, longitude: null),
                new (street: "Calle", streetNumber: 60, intersectionNumber: 8, doorNumber: 6, complement: null, latitude: null, longitude: null),
                new (street: "Avenida", streetNumber: 70, intersectionNumber: 9, doorNumber: 7, complement: null, latitude: null, longitude: null),
                new (street: "Carrera", streetNumber: 80, intersectionNumber: 10, doorNumber: 8, complement: null, latitude: null, longitude: null),
                new (street: "Circular", streetNumber: 90, intersectionNumber: 11, doorNumber: 9, complement: null, latitude: null, longitude: null),
                new (street: "Transversal", streetNumber: 100, intersectionNumber: 12, doorNumber: 10, complement: null, latitude: null, longitude: null),
                new (street: "Diagonal", streetNumber: 110, intersectionNumber: 13, doorNumber: 11, complement: null, latitude: null, longitude: null),
                new (street: "Calle", streetNumber: 120, intersectionNumber: 14, doorNumber: 12, complement: null, latitude: null, longitude: null),
                new (street: "Avenida", streetNumber: 130, intersectionNumber: 15, doorNumber: 13, complement: null, latitude: null, longitude: null),
                new (street: "Carrera", streetNumber: 140, intersectionNumber: 16, doorNumber: 14, complement: null, latitude: null, longitude: null),
                new (street: "Circular", streetNumber: 150, intersectionNumber: 17, doorNumber: 15, complement: null, latitude: null, longitude: null),
            ];

            List<Properties> properties = [
                new (title: "Cozy Apartment in Downtown", description: "A cozy apartment located in the heart of the city, perfect for short stays.", numBathrooms: 1, numBedrooms: 1, numBeds: 1, maxGuests: 2, basePricePerNight: 50.00m, typeId: propertyTypes[0].Id, hostId: hosts[0].Id, addressId: addresses[0].Id, CityId: cities[0].Id, CountryId: countries[0].Id){
                    Type = propertyTypes[0],
                    Address = addresses[0],
                    City = cities[0],
                    Country = countries[0],
                    Host = hosts[0],
                },
                new (title: "Spacious House with Garden", description: "A spacious house with a beautiful garden, ideal for families.", numBathrooms: 2, numBedrooms: 3, numBeds: 3, maxGuests: 6, basePricePerNight: 120.00m, typeId: propertyTypes[1].Id, hostId: hosts[1].Id, addressId: addresses[1].Id, CityId: cities[1].Id, CountryId: countries[0].Id){
                    Type = propertyTypes[1],
                    Address = addresses[1],
                    City = cities[1],
                    Country = countries[0],
                    Host = hosts[1],
                },
                new (title: "Modern Condo with City View", description: "A modern condo with stunning city views, perfect for business travelers.", numBathrooms: 1, numBedrooms: 1, numBeds: 1, maxGuests: 2, basePricePerNight: 80.00m, typeId: propertyTypes[2].Id, hostId: hosts[0].Id, addressId: addresses[2].Id, CityId: cities[2].Id, CountryId: countries[0].Id) {
                    Type = propertyTypes[2],
                    Address = addresses[2],
                    City = cities[2],
                    Country = countries[0],
                    Host = hosts[0],
                },
                new (title: "Rustic Cabin in the Woods", description: "A rustic cabin surrounded by nature, ideal for a peaceful retreat.", numBathrooms: 1, numBedrooms: 2, numBeds: 2, maxGuests: 4, basePricePerNight: 70.00m, typeId: propertyTypes[3].Id, hostId: hosts[1].Id, addressId: addresses[3].Id, CityId: cities[3].Id, CountryId: countries[0].Id) {
                    Type = propertyTypes[3],
                    Address = addresses[3],
                    City = cities[3],
                    Country = countries[0],
                    Host = hosts[1],
                },
                new (title: "Stylish Loft in Arts District", description: "A stylish loft located in the arts district, perfect for creatives.", numBathrooms: 1, numBedrooms: 1, numBeds: 1, maxGuests: 2, basePricePerNight: 90.00m, typeId: propertyTypes[4].Id, hostId: hosts[0].Id, addressId: addresses[4].Id, CityId: cities[4].Id, CountryId: countries[0].Id) {
                    Type = propertyTypes[4],
                    Address = addresses[4],
                    City = cities[4],
                    Country = countries[0],
                    Host = hosts[0],
                },
                new (title: "Luxury Villa with Pool", description: "A luxury villa with a private pool, perfect for a lavish getaway.", numBathrooms: 3, numBedrooms: 4, numBeds: 4, maxGuests: 8, basePricePerNight: 250.00m, typeId: propertyTypes[5].Id, hostId: hosts[1].Id, addressId: addresses[5].Id, CityId: cities[0].Id, CountryId: countries[0].Id) {
                    Type = propertyTypes[5],
                    Address = addresses[5],
                    City = cities[0],
                    Country = countries[0],
                    Host = hosts[1],
                },
                new (title: "Charming Cottage by the Sea", description: "A charming cottage located by the sea, ideal for beach lovers.", numBathrooms: 1, numBedrooms: 2, numBeds: 2, maxGuests: 4, basePricePerNight: 100.00m, typeId: propertyTypes[0].Id, hostId: hosts[0].Id, addressId: addresses[6].Id, CityId: cities[1].Id, CountryId: countries[0].Id) {
                    Type = propertyTypes[0],
                    Address = addresses[6],
                    City = cities[1],
                    Country = countries[0],
                    Host = hosts[0],
                },
                new (title: "Elegant Apartment in Historic District", description: "An elegant apartment located in the historic district, perfect for culture enthusiasts.", numBathrooms: 1, numBedrooms: 1, numBeds: 1, maxGuests: 2, basePricePerNight: 75.00m, typeId: propertyTypes[1].Id, hostId: hosts[1].Id, addressId: addresses[7].Id, CityId: cities[2].Id, CountryId: countries[0].Id) {
                    Type = propertyTypes[1],
                    Address = addresses[7],
                    City = cities[2],
                    Country = countries[0],
                    Host = hosts[1],
                },
                new (title: "Contemporary House with Pool", description: "A contemporary house with a private pool, ideal for summer vacations.", numBathrooms: 2, numBedrooms: 3, numBeds: 3, maxGuests: 6, basePricePerNight: 150.00m, typeId: propertyTypes[2].Id, hostId: hosts[0].Id, addressId: addresses[8].Id, CityId: cities[3].Id, CountryId: countries[0].Id) {
                    Type = propertyTypes[2],
                    Address = addresses[8],
                    City = cities[3],
                    Country = countries[0],
                    Host = hosts[0]
                },
                new (title: "Quaint Cabin in the Mountains", description: "A quaint cabin located in the mountains, perfect for hiking enthusiasts.", numBathrooms: 1, numBedrooms: 2, numBeds: 2, maxGuests: 4, basePricePerNight: 80.00m, typeId: propertyTypes[3].Id, hostId: hosts[1].Id, addressId: addresses[9].Id, CityId: cities[4].Id, CountryId: countries[0].Id) {
                    Type = propertyTypes[3],
                    Address = addresses[9],
                    City = cities[4],
                    Country = countries[0],
                    Host = hosts[1]
                },
                new (title: "Modern Loft with Rooftop Terrace", description: "A modern loft featuring a rooftop terrace, ideal for urban explorers.", numBathrooms: 1, numBedrooms: 1, numBeds: 1, maxGuests: 2, basePricePerNight: 110.00m, typeId: propertyTypes[4].Id, hostId: hosts[0].Id, addressId: addresses[10].Id, CityId: cities[0].Id, CountryId: countries[0].Id) {
                    Type = propertyTypes[4],
                    Address = addresses[10],
                    City = cities[0],
                    Country = countries[0],
                    Host = hosts[0]
                },
                new (title: "Luxury Villa with Ocean View", description: "A luxury villa with breathtaking ocean views, perfect for a romantic getaway.", numBathrooms: 3, numBedrooms: 4, numBeds: 4, maxGuests: 8, basePricePerNight: 300.00m, typeId: propertyTypes[5].Id, hostId: hosts[1].Id, addressId: addresses[11].Id, CityId: cities[1].Id, CountryId: countries[0].Id) {
                    Type = propertyTypes[5],
                    Address = addresses[11],
                    City = cities[1],
                    Country = countries[0],
                    Host = hosts[1]
                },
                new (title: "Charming Cottage in the Countryside", description: "A charming cottage located in the countryside, ideal for a peaceful retreat.", numBathrooms: 1, numBedrooms: 2, numBeds: 2, maxGuests: 4, basePricePerNight: 90.00m, typeId: propertyTypes[0].Id, hostId: hosts[0].Id, addressId: addresses[12].Id, CityId: cities[2].Id, CountryId: countries[0].Id) {
                    Type = propertyTypes[0],
                    Address = addresses[12],
                    City = cities[2],
                    Country = countries[0],
                    Host = hosts[0]
                },
                new (title: "Elegant Apartment with Balcony", description: "An elegant apartment featuring a balcony, perfect for enjoying the city views.", numBathrooms: 1, numBedrooms: 1, numBeds: 1, maxGuests: 2, basePricePerNight: 85.00m, typeId: propertyTypes[1].Id, hostId: hosts[1].Id, addressId: addresses[13].Id, CityId: cities[3].Id, CountryId: countries[0].Id) {
                    Type = propertyTypes[1],
                    Address = addresses[13],
                    City = cities[3],
                    Country = countries[0],
                    Host = hosts[1]
                },
                new (title: "Contemporary House with Garden", description: "A contemporary house with a spacious garden, ideal for family gatherings.", numBathrooms: 2, numBedrooms: 3, numBeds: 3, maxGuests: 6, basePricePerNight: 130.00m, typeId: propertyTypes[2].Id, hostId: hosts[0].Id, addressId: addresses[14].Id, CityId: cities[4].Id, CountryId: countries[0].Id) {
                    Type = propertyTypes[2],
                    Address = addresses[14],
                    City = cities[4],
                    Country = countries[0],
                    Host = hosts[0]
                },
            ];

            List<PropertyAssets> propertyAssets = [
                new () {
                    Url = new Uri("https://firebasestorage.googleapis.com/v0/b/prueba-skybnb-universidad/o/image%2F1_CozyAparment.jpg?alt=media&token=e4caca84-ea00-4c1d-88d5-b6584c07cc91"),
                    Type = "image",
                    Order = 1,
                    Property = properties[0]
                },
                new () {
                    Url = new Uri("https://firebasestorage.googleapis.com/v0/b/prueba-skybnb-universidad/o/image%2F2_CozyAparment.jpg?alt=media&token=a73469c8-ce50-4efa-8a43-e2304cdc389f"),
                    Type = "image",
                    Order = 2,
                    Property = properties[0]
                },
                new () {
                    Url = new Uri("https://firebasestorage.googleapis.com/v0/b/prueba-skybnb-universidad/o/image%2F3_HouseWithGarden.jpg?alt=media&token=021f88d3-5752-4b32-b928-954d33ee5c90"),
                    Type = "image",
                    Order = 1,
                    Property = properties[1]
                },
                new () {
                    Url = new Uri("https://firebasestorage.googleapis.com/v0/b/prueba-skybnb-universidad/o/image%2F4_HouseWithGarden.jfif?alt=media&token=fc74c5f5-6caf-4cb3-af6f-92e952038f31"),
                    Type = "image",
                    Order = 2,
                    Property = properties[1]
                },
                new () {
                    Url = new Uri("https://firebasestorage.googleapis.com/v0/b/prueba-skybnb-universidad/o/image%2F5_ModernCondo.webp?alt=media&token=2d2cf880-28d2-4183-83d8-b686f716f1ca"),
                    Type = "image",
                    Order = 1,
                    Property = properties[2]
                },
                new () {
                    Url = new Uri("https://firebasestorage.googleapis.com/v0/b/prueba-skybnb-universidad/o/image%2F6_ModernCondo.jpg?alt=media&token=f9038cd0-f8bc-446a-ac45-ca6d737edcb2"),
                    Type = "image",
                    Order = 2,
                    Property = properties[2]
                },
                new () {
                    Url = new Uri("https://firebasestorage.googleapis.com/v0/b/prueba-skybnb-universidad/o/image%2F7_RusticCabin.jfif?alt=media&token=bdf85bd2-cd78-4bd2-bfdf-cc69d4650ad4"),
                    Type = "image",
                    Order = 1,
                    Property = properties[3]
                },
                new () {
                    Url = new Uri("https://firebasestorage.googleapis.com/v0/b/prueba-skybnb-universidad/o/image%2F8_RusticCabin.jpg?alt=media&token=a1e3db9c-e0b9-4f1d-986a-2f6dca67aa17"),
                    Type = "image",
                    Order = 2,
                    Property = properties[3]
                },
                new () {
                    Url = new Uri("https://firebasestorage.googleapis.com/v0/b/prueba-skybnb-universidad/o/image%2F9_StylishLoft.jpeg?alt=media&token=baf42a50-e25f-4a53-b174-05ffc55a3115"),
                    Type = "image",
                    Order = 1,
                    Property = properties[4]
                },
                new () {
                    Url = new Uri("https://firebasestorage.googleapis.com/v0/b/prueba-skybnb-universidad/o/image%2F10_StylishLoft.jpg?alt=media&token=77616646-9989-462d-a5e2-113c99a7968f"),
                    Type = "image",
                    Order = 2,
                    Property = properties[4]
                },
                new () {
                    Url = new Uri("https://firebasestorage.googleapis.com/v0/b/prueba-skybnb-universidad/o/image%2F11_LuxuryVilla.jpg?alt=media&token=c5f45310-788f-4d74-8d67-e575ff185867"),
                    Type = "image",
                    Order = 1,
                    Property = properties[5]
                },
                new () {
                    Url = new Uri("https://firebasestorage.googleapis.com/v0/b/prueba-skybnb-universidad/o/image%2F12_LuxuryVilla.jpeg?alt=media&token=42879d38-5d27-47bb-852d-06ea544cb68e"),
                    Type = "image",
                    Order = 2,
                    Property = properties[5]
                },
                new (){
                    Url = new Uri("https://firebasestorage.googleapis.com/v0/b/prueba-skybnb-universidad/o/image%2F13_CharmingCottage.jpg?alt=media&token=b074c3e1-f866-4e21-ad37-72b77abe3d83"),
                    Type = "image",
                    Order = 1,
                    Property = properties[6]
                },
                new () {
                    Url = new Uri("https://firebasestorage.googleapis.com/v0/b/prueba-skybnb-universidad/o/image%2F14_CharmingCottage.webp?alt=media&token=1d131a2e-33b8-4491-867a-5c368ee0d543"),
                    Type = "image",
                    Order = 2,
                    Property = properties[6]
                },
                new () {
                    Url = new Uri("https://firebasestorage.googleapis.com/v0/b/prueba-skybnb-universidad/o/image%2F15_ElegantApartment.jpg?alt=media&token=188c8116-8dcf-4c5a-910f-c1a8ef591bb6"),
                    Type = "image",
                    Order = 1,
                    Property = properties[7]
                },
                new () {
                    Url = new Uri("https://firebasestorage.googleapis.com/v0/b/prueba-skybnb-universidad/o/image%2F16_ElegantApartment.jfif?alt=media&token=fcf025aa-8781-4c88-91fc-537030e15ee3"),
                    Type = "image",
                    Order = 2,
                    Property = properties[7]
                }
            ];

            await _conexion.Properties.AddRangeAsync(properties);
            await _conexion.PropertyAssets.AddRangeAsync(propertyAssets);
            await _conexion.SaveChangesAsync();

            Console.WriteLine($"{properties.Count} properties initialized successfully.");
        }
    }
}
