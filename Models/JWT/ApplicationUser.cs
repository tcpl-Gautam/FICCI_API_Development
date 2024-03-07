using Microsoft.AspNetCore.Identity;

namespace FICCI_API.Models.JWT
{
    public class ApplicationUser : IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }



    public static class UserRoles
    {
        public const string Admin = "Admin";
        public const string User = "User";
    }


    public class TokenModel
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
