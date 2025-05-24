namespace domain.Entities
{
    public interface ISoftDeletable
    {
        bool IsActive { get; set; }
    }
}
