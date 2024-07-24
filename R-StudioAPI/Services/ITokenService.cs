using R_StudioAPI.Models;

namespace R_StudioAPI.Services
{
    public interface ITokenService
    {
        Task<string> CreateToken(User user);
    }
}
