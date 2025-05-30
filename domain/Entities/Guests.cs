using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace domain.Entities
{
    public class Guests(
        Guid customerId,
        Guid addressId
        )
    {
        [Key]
        [ForeignKey("Customers")]
        public Guid CustomerId { get; private set; } = customerId;

        public Guid AddressId { get; set; } = addressId;


        public Addresses? Address { get; set; }
        public Customers? Customer { get; set; }
    }
}
