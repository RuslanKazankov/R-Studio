using R_StudioAPI.Models;

namespace R_StudioAPI.Services
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
