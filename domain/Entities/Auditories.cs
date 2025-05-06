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
        public long Id { get; private set; }

        public Guid UserId { get; private set; } = userId;

        public string Action { get; private set; } = action;

        public DateTime Timestamp { get; private set; } = timestamp;

        public string? Entity { get; private set; } = entity;

        public string? EntityId { get; private set; } = entityId;

        public string? Details { get; private set; } = details;
        

        public Users? User { get; set; }
    }
}
