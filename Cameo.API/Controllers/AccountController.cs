using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Cameo.Common;
using Cameo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Cameo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost("Authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] LoginVM login)
        {
            var user = await _userManager.FindByNameAsync(login.Username);
            if (user == null)
                return NotFound("User not found");

            var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, true);
            //var result = await _signInManager.PasswordSignInAsync(login.Username, login.Password, login.RememberMe, lockoutOnFailure: true);
            if (result.Succeeded)
            {
                string tokenSecurityKey = AppData.Configuration.TokenSecurityKey;
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSecurityKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, login.Username),
                    new Claim(ClaimTypes.Role, "Administrator"),
                };
#if DEBUG
                string validIssuer = AppData.Configuration.TokenValidIssuer;
                string validAudience = AppData.Configuration.TokenValidAudience;
#else
                string validIssuer = AppData.Configuration.TokenValidIssuerServer;
                string validAudience = AppData.Configuration.TokenValidAudienceServer;
#endif
                int tokenExpirationPeriodInDays = AppData.Configuration.TokenExpirationPeriodInDays;
                var token = new JwtSecurityToken(validIssuer, validAudience, claims, expires: DateTime.Now.AddDays(tokenExpirationPeriodInDays), signingCredentials: creds);

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                return new JsonResult(new { token = tokenString });
            }
            else
                return BadRequest("Bad password");
        }
    }

    public class LoginVM
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}