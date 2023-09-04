using IUstaProject.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IUstaProject.Services
{
    public static class JWTTokenService
    {
        public static string CreateToken(User admin)
        {

            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name,admin.UserName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Myjhzvdgkszjhvgzksjgvzsjgdvkszjhvzksjgvdkzjsvg"));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow,
                signingCredentials: creds
            );

            var JWT = new JwtSecurityTokenHandler().WriteToken(token);
            return JWT;
        }

    }
}
