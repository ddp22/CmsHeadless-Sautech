using CmsHeadless.Models;
using Microsoft.AspNetCore.Identity;

namespace CmsHeadless.AuthenticationJWT
{
    public interface ITokenService
    {
        string BuildToken(string key, string issuer, CmsUser user, IdentityRole role);
        bool ValidateToken(string key, string issuer, string audience, string token);
    }

}
