namespace application.Core
{
    public class JwtOptions
    {
        public string SecretKey { get; set; } = string.Empty;
        public int TokenExpirationInMinutes { get; set; }
    }
}
