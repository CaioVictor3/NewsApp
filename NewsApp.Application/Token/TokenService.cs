using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace NewsApp.Application.Token
{
    public static class TokenService
    {
        public static string GenerateToken(string userName, string role, int idUsuario, string nome)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("11ccc561fdbf0dc949f2a7739606973e94d915b971b250d530e43ff651e8db1d"); 
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                   new Claim(ClaimTypes.Name, userName),
                   new Claim(ClaimTypes.Role, role),
                   new Claim("idUsuario",idUsuario.ToString()),
                   new Claim("nome",nome.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
