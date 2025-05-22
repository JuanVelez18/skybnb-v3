namespace repository.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        IUserRepository Users { get; }
        IGuestRepository Guests { get; }
        IAddressRepository Addresses { get; }

        Task<int> CommitAsync();
    }
}
