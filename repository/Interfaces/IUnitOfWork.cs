namespace repository.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        IUserRepository Users { get; }
        IGuestRepository Guests { get; }
        IAddressRepository Addresses { get; }
        IRoleRepository Roles { get; }
        IPropertiesRepository Properties { get; }
        IReviewsRepository Reviews { get; }
        IBookingsRepository Bookings { get; }
        IAuditoryRepository Auditories { get; }

        Task<int> CommitAsync();
    }
}
