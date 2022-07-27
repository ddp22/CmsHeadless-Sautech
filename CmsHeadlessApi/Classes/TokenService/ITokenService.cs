using CmsHeadlessApi.Models;

namespace CmsHeadlessApi.Classes.TokenService
{
    public interface ITokenService
    {
        string BuildToken(string key, string issuer, UserDTO user);
        bool ValidateToken(string key, string issuer, string audience, string token);
    }
}
