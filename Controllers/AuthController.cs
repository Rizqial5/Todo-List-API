using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TodoListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AuthController(IConfiguration configuration, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _configuration = configuration;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            // Cari user berdasarkan username (atau email, jika menggunakan email untuk login)
            var user = await _userManager.FindByNameAsync(login.Username);
            if (user == null)
            {
                return Unauthorized("Invalid credentials");
            }

            // Cek apakah password yang dimasukkan valid
            var result = await _signInManager.PasswordSignInAsync(user, login.Password, false, false);
            if (result.Succeeded)
            {
                // Generate token jika login berhasil
                var token = GenerateJwtToken(login.Username);
                return Ok(new { Token = token });
            }

            return Unauthorized("Invalid credentials");
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel register)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = register.Username,
                    Email = register.Email
                };

                var result = await _userManager.CreateAsync(user, register.Password);
                if (result.Succeeded)
                {
                    // Jika registrasi berhasil, login dan buat token JWT
                    var token = GenerateJwtToken(register.Username);
                    return Ok(new { Token = token });
                }

                return BadRequest(result.Errors);
            }

            return BadRequest("Invalid data.");
        }


        private string GenerateJwtToken(string username)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_very_long_secret_key_here_32_chars"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, "User")
            };

            var token = new JwtSecurityToken(
                issuer: "AppName",
                audience: "AppUser",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }


    }

    public class LoginModel
    {
        public string Username {get; set;}
        public string Password {get; set;}
    }

    public class RegisterModel
    {
        public string Email {get; set;}
        public string Username {get; set;}
        public string Password {get; set;}
    }
}
