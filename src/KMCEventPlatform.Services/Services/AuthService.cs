using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using KMCEventPlatform.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace KMCEventPlatform.Services.Services
{
    public record AuthTokenResult(string Token, DateTime ExpiresAtUtc);

    public interface IAuthService
    {
        string HashPassword(string password);
        bool VerifyPassword(string passwordHash, string password);
        AuthTokenResult GenerateToken(Participant participant);
    }

    public class AuthService : IAuthService
    {
        private const int SaltSize = 16;
        private const int KeySize = 32;
        private const int Iterations = 100000;
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string HashPassword(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(SaltSize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, KeySize);
            return $"{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
        }

        public bool VerifyPassword(string passwordHash, string password)
        {
            var parts = passwordHash.Split('.', 2);
            if (parts.Length != 2)
                return false;

            var salt = Convert.FromBase64String(parts[0]);
            var expectedHash = Convert.FromBase64String(parts[1]);
            var actualHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, KeySize);

            return CryptographicOperations.FixedTimeEquals(expectedHash, actualHash);
        }

        public AuthTokenResult GenerateToken(Participant participant)
        {
            var secret = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT key is missing.");
            var issuer = _configuration["Jwt:Issuer"] ?? "KMCEventPlatform";
            var audience = _configuration["Jwt:Audience"] ?? "KMCEventPlatform.Client";
            var expiresMinutes = int.TryParse(_configuration["Jwt:ExpiresMinutes"], out var minutes) ? minutes : 120;
            var expiresAtUtc = DateTime.UtcNow.AddMinutes(expiresMinutes);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, participant.Id),
                new(ClaimTypes.Name, participant.FullName),
                new(ClaimTypes.Email, participant.Email),
                new(ClaimTypes.Role, participant.Role.ToString())
            };

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiresAtUtc,
                signingCredentials: credentials);

            return new AuthTokenResult(new JwtSecurityTokenHandler().WriteToken(token), expiresAtUtc);
        }
    }
}
