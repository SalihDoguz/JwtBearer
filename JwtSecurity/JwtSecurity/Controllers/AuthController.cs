using JwtSecurity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtSecurity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtSettings _jwtSettings;
        public AuthController(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }


        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserApi userApiInfo)
        {
            var userApi = AuthenticationControl(userApiInfo);

            if (userApi == null)
            {
                return NotFound("Kullanıcı Bulunamadı");
            }
  
            var token = CreateToken(userApi);
            return Ok(token);
        }

        private string CreateToken(UserApi userApi)
        {
            if (_jwtSettings.Key == null) throw new Exception("Jwt ayarlarındaki Key değeri null olamaz!");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claimDizi = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,userApi.UserName!),
                new Claim(ClaimTypes.Role,userApi.Role!)
            };

            var token = new JwtSecurityToken(_jwtSettings.Issuer,
                _jwtSettings.Audience,
                claimDizi,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
                );


            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UserApi? AuthenticationControl(UserApi userApiInfo)
        {
            return UsersApi.Users.FirstOrDefault(x =>
            x.UserName?.ToLower() == userApiInfo.UserName &&
            x.Password == userApiInfo.Password);
        }

    }
}

