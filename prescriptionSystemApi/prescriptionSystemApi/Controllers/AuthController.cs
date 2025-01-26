using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using prescriptionSystemApi.model;
using prescriptionSystemApi.model.dto;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;

namespace prescriptionSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            var user = Authenticate(dto);
            if (user != null)
            {
                var token = GenerateToken(user);
                return Ok(new { Token=token, Role = user.Role ,Id=user.Id, Name=user.Name});
            }
            return Unauthorized("Invalid username or password");
        }

        private User Authenticate(LoginDto dto)
        {
            Console.WriteLine($"Authenticating Username: {dto.Username}, Password: {dto.Password}");
            var user = UserConstants.Users.FirstOrDefault(u=>u.Username.Equals(dto.Username,StringComparison.OrdinalIgnoreCase) && u.Password == dto.Password);
            if (user != null)
            {
                Console.WriteLine($"User authenticated: {user.Username} ({user.Role})");
            }
            else
            {
                Console.WriteLine("No matching user found.");
            }
            return user;
        }

        private string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("Name", user.Name),
                new Claim("Email", user.Email ?? string.Empty)
            };
            var token = new JwtSecurityToken(
               issuer: _config["Jwt:Issuer"],
               audience: _config["Jwt:Audience"],
               claims: claims,
               expires: DateTime.Now.AddHours(1),
               signingCredentials: credentials);
            Console.WriteLine("Generating token with claims:");
            foreach (var claim in claims)
            {
                Console.WriteLine($"{claim.Type}: {claim.Value}");
            }
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        
    }
}
