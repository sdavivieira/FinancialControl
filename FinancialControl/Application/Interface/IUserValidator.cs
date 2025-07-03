using FinancialControl.ResponseRequest.Request.User;

namespace FinancialControl.Application.Interface
{
    public interface IUserValidator
    {
        Task<bool> IsValid(LoginRequest user);
        Task<string> GenerateJWT();
    }
}
