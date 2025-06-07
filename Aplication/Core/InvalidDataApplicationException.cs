namespace application.Core
{
    public class InvalidDataApplicationException : ApplicationException
    {
        public InvalidDataApplicationException(string message) : base(message)
        {
        }

        public InvalidDataApplicationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
