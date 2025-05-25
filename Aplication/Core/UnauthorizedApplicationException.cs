namespace application.Core
{
    public class UnauthorizedApplicationException: ApplicationException
    {
        public UnauthorizedApplicationException(string message) : base(message)
        {
        }
        public UnauthorizedApplicationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
