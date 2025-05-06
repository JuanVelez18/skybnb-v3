using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace domain.Entities
{
    [Index(nameof(AddressId), IsUnique = true)]
    public class Properties(
        string title,
        string description,
        int numBathrooms,
        int numBedrooms,
        int numBeds,
        int maxGuests,
        decimal basePricePerNight,
        int typeId,
        Guid hostId,
        Guid addressId
        )
    {
        public Guid Id { get; private set; }

        [MaxLength(150)]
        public string Title { get; set; } = title;

        [MaxLength(2000)]
        public string Description { get; set; } = description;

        [Range(0, int.MaxValue)]
        public int NumBathrooms { get; set; } = numBathrooms;

        [Range(0, int.MaxValue)]
        public int NumBedrooms { get; set; } = numBedrooms;

        [Range(0, int.MaxValue)]
        public int NumBeds { get; set; } = numBeds;

        [Range(0, int.MaxValue)]
        public int MaxGuests { get; set; } = maxGuests;

        [Precision(12,2)]
        public decimal BasePricePerNight { get; set; } = basePricePerNight;

        public int TypeId { get; set; } = typeId;

        public Guid HostId { get; private set; } = hostId;

        public Guid AddressId { get; set; } = addressId;

        [Range(0, int.MaxValue)]
        public int ReviewsCount { get; set; } = 0;

        [Precision(2,1)]
        public decimal? AverageRating { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;


        public PropertyTypes? Type { get; set; }
        public Users? Host { get; set; }
        public Addresses? Address { get; set; }
        public List<Bookings> Bookings { get; set; } = [];
        public List<Reviews> Reviews { get; set; } = [];
    }
}
