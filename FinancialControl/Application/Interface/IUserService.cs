using FinancialControl.ResponseRequest;
using FinancialControl.ResponseRequest.Response.User;
using FinancialControl.ResponseRequest.Request.User;

namespace FinancialControl.Application.Interface
{
    public interface IUserService
    {
        Task<OperationResult<UserResponse>> Create(UserRequest user);
        Task<OperationResult<ProfileResponse>> Profile(string email);
        Task<OperationResult<ProfileResponse>> ProfileSave(ProfileRequest profile);
    }
}
