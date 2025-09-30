using FinancialControl.Application.Interface;
using FinancialControl.ResponseRequest.Request.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinancialControl.Controllers
{
    public class AuthController : BaseControllerapi
    {
        private readonly IUserValidator _userValidator;

        public AuthController(IUserValidator userValidator)
        {
            _userValidator = userValidator;     
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest user)
        {
            bool result = await _userValidator.IsValid(user);

            if (!result)
                return Unauthorized();

            var token = await _userValidator.GenerateJWT(user);

            Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, 
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddMinutes(120)
            });

            return Ok(new { token });
        }


        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> Me()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var email = User.FindFirstValue(ClaimTypes.Email);
            var name = User.FindFirstValue(ClaimTypes.Name);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "Token inválido" });
            }

            return Ok(new
            {
                Id = userId,
                Name = name,
                Email = email
            });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");
            return Ok(new { message = "Logout successful" });
        }

    }
}
