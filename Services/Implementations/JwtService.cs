using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VehiclePartsIMS_Backend.Services.Interfaces;

namespace VehiclePartsIMS_Backend.Services.Implementations
{
    public class JwtService(IConfiguration config) : IJwtService
    {
        public string GenerateToken(int userId, string name, string email, string role)
        {
            var jwtOptions = config.GetSection("JWT");

            var secretKey = jwtOptions["SecretKey"]!;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenObj = new JwtSecurityToken(
                issuer: jwtOptions["Issuer"],
                audience: jwtOptions["Audience"],
                claims: [
                    new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                    new Claim(JwtRegisteredClaimNames.Name, name),
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Role, role)
                ],
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtOptions["ExpiresInMinutes"]!)),
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenObj);
        }
    }
}
