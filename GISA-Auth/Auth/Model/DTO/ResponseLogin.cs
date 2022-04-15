namespace GISA_Auth.Auth.Model.DTO
{
    public class ResponseLogin
    {
        public string? Token { get; set; }
        public DateTime Expiration { get; set; }
        public UserResponse? User { get; set; }
    }

    public class UserResponse
    {
        public string Id { get; set; }
        public string? Username { get; set; }
        public int Role { get; set; }
    }
}
