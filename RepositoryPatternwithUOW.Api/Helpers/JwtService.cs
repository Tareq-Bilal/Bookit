using Microsoft.IdentityModel.Tokens;
using RepositoryPatternwithUOW.Api.DTO_s.Authentication;
using RepositoryPatternWithUOW.Core.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core.Helpers
{
    public class JwtService
    {
        public string ValidAudience { get; set; }
        public string ValidIssuer { get; set; }
        public string Key { get; set; }
        public int ExpiryMinutes { get; set; }

        public static string GetSecureToken(JwtService jwtService, User user)
        {
            return _GenerateSecureToken(jwtService , user);
        }
        public string GenerateToken(JwtService jwtOptions , LoginDTO dto)
        {
             JwtService _jwtOptions = jwtOptions ;
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {

                Issuer = _jwtOptions.ValidIssuer,
                Audience = _jwtOptions.ValidAudience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key))
                , SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new (ClaimTypes.Email , dto.Email)
                })

            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);
            return accessToken;
        }

        private static string _GenerateSecureToken(JwtService jwtService , User user)
        {
            JwtService _jwtOptions = jwtService;

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _jwtOptions.ValidIssuer,
                Audience = _jwtOptions.ValidAudience,
                Expires = DateTime.UtcNow.AddHours(24), // ✅ Add expiration
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key)),
                    SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Name ?? ""),
            // Add roles if needed
            // new Claim(ClaimTypes.Role, user.Role)
        })
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(securityToken);
        }

    }
}
