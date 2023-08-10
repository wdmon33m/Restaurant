using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Restaurant.Services.AuthAPI.Models;
using Restaurant.Services.AuthAPI.Service.IService;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Restaurant.Services.AuthAPI.Service
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JWTOptions _jWTOptions;

        public JwtTokenGenerator(IOptions<JWTOptions> jWTOptions)
        {
            _jWTOptions = jWTOptions.Value;
        }

        public string GenerateToken(ApplicationUser applicationUser)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jWTOptions.Secret);

            var cliamList = new List<Claim>
            {
                new Claim (JwtRegisteredClaimNames.Email, applicationUser.Email),
                new Claim (JwtRegisteredClaimNames.Sub, applicationUser.Id),
                new Claim (JwtRegisteredClaimNames.Name, applicationUser.UserName)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _jWTOptions.Audience,
                Issuer = _jWTOptions.Issuer,
                Subject = new ClaimsIdentity(cliamList),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials
                                    (new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
