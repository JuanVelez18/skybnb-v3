using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace domain.Entities
{
    public class Guests(
        Guid customerId,
        Guid addressId,
        int CityId,
        int CountryId
    )
    {
        [Key]
        [ForeignKey("Customers")]
        public Guid CustomerId { get; private set; } = customerId;

        public Guid AddressId { get; set; } = addressId;
        public int CityId { get; set; } = CityId;
        public int CountryId { get; set; } = CountryId;


        public Customers? Customer { get; set; }
        public Addresses? Address { get; set; }
        public Cities? City { get; set; }
        public Countries? Country { get; set; }
    }
}
