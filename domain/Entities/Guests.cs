using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace domain.Entities
{
    public class Guests(
        Guid userId,
        Guid addressId
        )
    {
        [Key]
        [ForeignKey("Users")]
        public Guid UserId { get; private set; } = userId;

        public Guid AddressId { get; set; } = addressId;


        public Addresses? Address { get; set; }
    }
}
