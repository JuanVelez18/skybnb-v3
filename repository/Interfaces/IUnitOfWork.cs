namespace repository.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        IUserRepository Users { get; }

        Task<int> CommitAsync();
    }
}
