namespace repository.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IGuestRepository Guests { get; }
        IAddressRepository Addresses { get; }
        IRoleRepository Roles { get; }
        IRefreshTokenRepository RefreshTokens { get; }
        ICountryRepository Countries { get; }
        ICityRepository Cities { get; }
        IAuditoryRepository Auditories { get; }

        Task<int> CommitAsync();
    }
}
