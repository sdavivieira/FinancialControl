using FinancialControl.Application.Interface;
using FinancialControl.Domain.Models;
using FinancialControl.ResponseRequest;
using FinancialControl.ResponseRequest.Request.User;
using FinancialControl.ResponseRequest.Response.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialControl.Controllers
{
    public class UserController : BaseControllerapi
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost("create")]
        public async Task<IActionResult> Create(UserRequest user)
        {
            if (user == null)
                return BadRequest();

            OperationResult<UserResponse> response = await _userService.Create(user);

            return Ok(response);
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> Profile([FromQuery] string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return BadRequest();

            OperationResult<ProfileResponse> response = await _userService.Profile(email);

            return Ok(response);
        }

        [Authorize]
        [HttpPost("profile")]
        public async Task<IActionResult> ProfileSave([FromBody] ProfileRequest profile)
        {
            var user = HttpContext.User;
            profile.Name = !string.IsNullOrWhiteSpace(profile.Name) ? profile.Name : user.FindFirst("name")?.Value!;
            profile.Email = !string.IsNullOrWhiteSpace(profile.Email) ? profile.Email : user.FindFirst("email")?.Value!;
            OperationResult<ProfileResponse> response = await _userService.ProfileSave(profile);

            return Ok(response);
        }

    }
}
