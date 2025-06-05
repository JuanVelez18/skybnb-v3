namespace application.Core
{
    public class ConflictApplicationException : ApplicationException
    {
        public ConflictApplicationException(string message) : base(message)
        {
        }

        public ConflictApplicationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}