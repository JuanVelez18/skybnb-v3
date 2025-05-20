namespace repository.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        IUserRepository Users { get; }
        IGuestRepository Guests { get; }

        Task<int> CommitAsync();
    }
}
