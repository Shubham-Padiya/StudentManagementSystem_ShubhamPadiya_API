using Microsoft.IdentityModel.Tokens;
using StudentManagement.Application.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentManagement.API.Auth
{
    public static class JWTGenerate
    {
        public static string GenerateToken(IConfiguration config, LoginDTO dto)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, dto.Username),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(config["Jwt:Key"]!)
            );

            var audiences = config.GetSection("Jwt:Audiences").Get<string[]>();

            foreach (var audience in audiences!)
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.Aud, audience));
            }

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    Convert.ToDouble(config["Jwt:ExpiryMinutes"])
                ),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
