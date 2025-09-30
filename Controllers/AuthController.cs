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

            if (result)
            {
                var token = await _userValidator.GenerateJWT(user);
                Response.Cookies.Append("jwt", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, 
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddMinutes(120)
                });

                return Ok(new { message = "Login successful" });
            }
            else
            {
                return Unauthorized();
            }
        }
        [HttpGet("me")]
        [Authorize]
        public IActionResult Me()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var email = User.FindFirstValue(ClaimTypes.Email);

            return Ok(new
            {
                Id = userId,
                Email = email
            });
        }
    }
}
