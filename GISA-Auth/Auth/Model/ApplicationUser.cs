using Microsoft.AspNetCore.Identity;

namespace GISA_Auth.Auth.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string Latitude { get; set; } = string.Empty;
        public string Longitude { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; }
        public DateTime LastAccess { get; set; }
    }
}
