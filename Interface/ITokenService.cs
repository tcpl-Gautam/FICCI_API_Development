using FICCI_API.DTO.Account;

namespace FICCI_API.Interface
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
        string CreateRefreshToken();
    }
}
