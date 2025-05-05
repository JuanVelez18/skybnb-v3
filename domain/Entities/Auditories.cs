using System.Numerics;
using Microsoft.EntityFrameworkCore;

namespace domain.Entities
{
    [Index(nameof(Timestamp))]
    [Index(nameof(UserId), nameof(Timestamp))]
    [Index(nameof(Action))]
    public class Auditories(
        Guid userId,
        string action,
        DateTime timestamp,
        string? entity,
        string? entityId,
        string? details
        )
    {
        public BigInteger Id { get; }

        public Guid UserId { get; } = userId;

        public string Action { get; } = action;

        public DateTime Timestamp { get; } = timestamp;

        public string? Entity { get; } = entity;

        public string? EntityId { get; } = entityId;

        public string? Details { get; } = details;
        

        public Users? User { get; set; }
    }
}
