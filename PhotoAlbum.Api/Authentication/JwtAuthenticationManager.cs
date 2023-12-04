using Microsoft.IdentityModel.Tokens;
using PhotoAlbum.Api.Repositories.Interfaces;
using PhotoAlbum.Shared.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PhotoAlbum.Api.Authentication
{
    public class JwtAuthenticationManager(IUserRepository userRepository, JwtSettings jwtSettings)
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly JwtSettings _jwtSettings = jwtSettings;

        public async Task<TokenModel?> GenerateJwtTokenAsync(string username, string password, bool rememberMe)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            var user = await _userRepository.AuthenticateUserAsync(username, password);
            if (user == null)
            {
                return null;
            }

            var tokenExpiryTimeStamp = rememberMe
                ? DateTime.UtcNow.AddMonths(1)
                : DateTime.UtcNow.AddMinutes(_jwtSettings.TokenMinutes);
            var tokenKey = Encoding.ASCII.GetBytes(_jwtSettings.Key);
            var claimsIdentity = new ClaimsIdentity(new List<Claim>
            {
                new(ClaimTypes.Name, user.Name),
                new(ClaimTypes.Role, user.Role)
            });

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature);
            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = tokenExpiryTimeStamp,
                SigningCredentials = signingCredentials,
                NotBefore = DateTime.UtcNow,
            };

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            var token = jwtSecurityTokenHandler.WriteToken(securityToken);

            TokenModel tokenModel = new()
            {
                UserId = user.UserId,
                Username = user.Username,
                Name = user.Name,
                Role = user.Role,
                Token = token,
                ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.UtcNow).TotalSeconds
            };

            return tokenModel;
        }
    }
}
