namespace application.Core
{
    public class NotFoundApplicationException: ApplicationException
    {
        public NotFoundApplicationException(string message) : base(message)
        {
        }
        public NotFoundApplicationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
