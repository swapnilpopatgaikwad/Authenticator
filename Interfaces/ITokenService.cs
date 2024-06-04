using Authenticator.Model;
using System;

namespace Authenticator.Interfaces
{
    public interface ITokenService
    {
        string GenerateRandomBase64Url(int bytesLength = 0x20);
        string CreateBase64Url(string text = "");
        string GenerateJwtToken(User user, out DateTime expires, double expireMinutes = 5);
    }
}
