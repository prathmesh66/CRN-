using System.Security.Cryptography;
namespace CRN.ProductApi.Infrastructure.Identity;
public static class PasswordService
{
 public static string Hash(string password){var salt=RandomNumberGenerator.GetBytes(16);var hash=Rfc2898DeriveBytes.Pbkdf2(password,salt,120000,HashAlgorithmName.SHA256,32);return $"PBKDF2$120000${Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";}
 public static bool Verify(string password,string encoded){var p=encoded.Split('$');if(p.Length!=4||p[0]!="PBKDF2")return false;var salt=Convert.FromBase64String(p[2]);var expected=Convert.FromBase64String(p[3]);var actual=Rfc2898DeriveBytes.Pbkdf2(password,salt,120000,HashAlgorithmName.SHA256,32);return CryptographicOperations.FixedTimeEquals(actual,expected);}
}
