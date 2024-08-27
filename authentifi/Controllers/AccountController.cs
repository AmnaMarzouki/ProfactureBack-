using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using authentifi.Models;
using Microsoft.IdentityModel.Tokens;
using Pro_FactureAPI.Service.Abonnement;

namespace authentifi.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IAbonnement _abonnement;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration configuration, IAbonnement abonnement)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _abonnement = abonnement;



		}

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new AppUser { UserName = model.Email, Email = model.Email, Name = model.Name };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var token = await GenerateJwtToken(user);
                return Ok(new { UserId = user.Id, Token = token });
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return BadRequest(ModelState);
        }


        private async Task<string> GenerateJwtToken(AppUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Id),
                    new Claim(ClaimTypes.Role, (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "User")
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Authentifier l'utilisateur
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, lockoutOnFailure: true);

            // Vérifier les résultats
            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                    return BadRequest("Unable to find user.");

                var token = await GenerateJwtToken(user);
                return Ok(new { UserId = user.Id, Token = token });
            }
            else if (result.IsLockedOut)
            {
                return StatusCode(StatusCodes.Status423Locked, "User account is locked out.");
            }
            else
            {
               
                return Unauthorized("Invalid login attempt.");
            }
        }


        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest("Invalid email address.");

            var token = await GeneratePasswordResetToken(user);
            return Ok(new { Token = token });
        }

        private async Task<string> GeneratePasswordResetToken(AppUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Id),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(1), // Expire dans 1 heure
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Validate JWT token and extract user ID
            var userId = ValidateResetToken(model.Token);
            if (userId == null)
                return BadRequest("Invalid token.");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return BadRequest("Invalid user.");

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, resetToken, model.NewPassword);

            if (result.Succeeded)
                return Ok();

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return BadRequest(ModelState);
        }

        private string ValidateResetToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Audience"]
                };

                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
                var userId = principal.FindFirst(ClaimTypes.Name)?.Value;

                return userId;
            }
            catch
            {
                return null;
            }
        }
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = _userManager.Users.ToList();
            if (users == null || !users.Any())
                return NotFound("No users found.");

            // Créez une liste d'objets anonymes ou d'un DTO pour les détails des utilisateurs
            var userDtos = users.Select(user => new
            {
                user.Id,
                user.UserName,
                user.Email,
                user.Name,
                
            }).ToList();

            return Ok(userDtos);
        }
        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound("User not found.");

            var userDto = new
            {
                user.Id,
                user.UserName,
                user.Email,
                user.Name
            };

            return Ok(userDto);

        }
    }
}
