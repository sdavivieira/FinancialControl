using FinancialControl.Application.Interface;
using FinancialControl.Domain.Interfaces.Users;
using FinancialControl.ResponseRequest.Request.User;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace FinancialControl.Application.UseCase
{
    public class UserValidator : IUserValidator
    {
        private IConfiguration _config;
        private readonly IUserReadRepository _userReadRepository;
        public UserValidator(IConfiguration config, IUserReadRepository userReadRepository)
        {
            _config = config;
            _userReadRepository = userReadRepository;
        }
        public async Task<string> GenerateJWT()
        {
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];
            var expiry = DateTime.Now.AddMinutes(120);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(issuer: issuer, audience: audience,
            expires: DateTime.Now.AddMinutes(120), signingCredentials: credentials);
            var tokenHandler = new JwtSecurityTokenHandler();
            var stringToken = tokenHandler.WriteToken(token);
            return stringToken;
        }

        public async Task<bool> IsValid(LoginRequest user)
        {
            if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
                return false;

            var existingUser = await _userReadRepository.GetAllAsync(x => x.Email == user.Email);

            if (existingUser == null)
                return false;


            return true;
        }

    }
}
