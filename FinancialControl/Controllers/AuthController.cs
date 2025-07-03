using FinancialControl.Application.Interface;
using FinancialControl.ResponseRequest.Request.User;
using Microsoft.AspNetCore.Mvc;

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
                var token = await _userValidator.GenerateJWT();
                return Ok(token);
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
