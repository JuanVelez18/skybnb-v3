namespace asp_services.Dtos
{
    public class UserDto
    {
        public string dni { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public DateOnly birthday { get; set; }
        public int countryId { get; set; }
        public string? phone { get; set; }
    }
}
