using System.Security.Cryptography;
using System.Text;

namespace Auth1796.Core.Application.Utilities;

public static class ServiceHelper
{
    const string alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

    public static string GeneratePassword(int length)
    {
        var secret = new StringBuilder();
        while (length-- > 0)
        {
            secret.Append(alphabet[RandomNumberGenerator.GetInt32(alphabet.Length)]);
        }
        return secret.ToString();
    }

}
