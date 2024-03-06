using FICCI_API.DTO.Account;
using System.Security.Claims;

namespace FICCI_API.Extension
{
    public class AuthenticUserDetails
    {
        public static JwtLoginDetailDto GetCurrentUserDetails(ClaimsIdentity identity)
        {
            if (identity != null)
            {
                var userClaims = identity.Claims;

                return new JwtLoginDetailDto
                {
                    EmpId = userClaims.FirstOrDefault(o => o.Type == "id")?.Value,
                    EmailId = userClaims.FirstOrDefault(o => o.Type == "email")?.Value,
                    Name = userClaims.FirstOrDefault(o => o.Type == "name")?.Value,
                    RoleName = userClaims.FirstOrDefault(o => o.Type == "role")?.Value,

                };
            }
            return null;
        }
    }
}
