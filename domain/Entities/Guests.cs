using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace domain.Entities
{
    public class Guests(
        Guid usuarioId,
        Guid addressId
        )
    {
        [Key]
        [ForeignKey("Users")]
        public Guid UserId { get; init; } = usuarioId;

        public Guid AddressId { get; set; } = addressId;


        public Addresses? Address { get; set; }
    }
}
