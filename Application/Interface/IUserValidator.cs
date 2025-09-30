using FinancialControl.Domain.Models;
using FinancialControl.ResponseRequest.Request.User;

namespace FinancialControl.Application.Interface
{
    public interface IUserValidator
    {
        Task<bool> IsValid(LoginRequest user);
        Task<string> GenerateJWT(LoginRequest user);
    }
}
