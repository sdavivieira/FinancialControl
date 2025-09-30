using FinancialControl.Application.Interface;
using FinancialControl.Domain.Interfaces.Users;
using FinancialControl.Domain.Models;
using FinancialControl.ResponseRequest.Request.User;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
        public async Task<string> GenerateJWT(LoginRequest user)
        {
            var existingUser = await _userReadRepository.GetAllAsync(x => x.Email == user.Email);
            var userLogin = existingUser.FirstOrDefault();

            if (userLogin == null)
                throw new Exception("Usuário não encontrado ou senha inválida");

            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
        new Claim(ClaimTypes.NameIdentifier, userLogin.Id.ToString()), 
        new Claim(ClaimTypes.Name, userLogin.Name),     
        new Claim(ClaimTypes.Email, userLogin.Email), 
        new Claim(JwtRegisteredClaimNames.Sub, userLogin.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(120), 
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
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
