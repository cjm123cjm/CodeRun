using CodeRun.Services.IService.Dtos.Outputs;
using CodeRun.Services.IService.Interfaces;
using CodeRun.Services.IService.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CodeRun.Services.Service.Implements
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtOptions _jwtOptions;

        public JwtTokenGenerator(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }
        public string GenerateToken(AccountDto account, List<string> permissionCodes)
        {
            var tokenHandle = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);

            var claimList = new List<Claim>
            {
                new Claim("UserName", account.UserName),
                new Claim("UserId",account.UserId.ToString()),
                new Claim("Roles",account.Roles!.ToString()),
                new Claim("IsAdmin",account.IsAdmin.ToString()),
            };

            foreach (var permission in permissionCodes)
            {
                claimList.Add(new Claim("Permission", permission));
            }

            var tokenDescript = new SecurityTokenDescriptor
            {
                Audience = _jwtOptions.Audience,
                Issuer = _jwtOptions.Issuer,
                Subject = new ClaimsIdentity(claimList),
                Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.Expires),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandle.CreateToken(tokenDescript);

            return tokenHandle.WriteToken(token);
        }
    }
}
