using FinancialControl.Application.Interface;
using FinancialControl.Domain.Interfaces.Profiles;
using FinancialControl.Domain.Interfaces.Users;
using FinancialControl.Domain.Models;
using FinancialControl.ResponseRequest;
using FinancialControl.ResponseRequest.Request.User;
using FinancialControl.ResponseRequest.Response.User;

namespace FinancialControl.Application.Service
{
    public class UserService : IUserService
    {
        private readonly IUserWriteRepository _writeuserrepository;
        private readonly IUserReadRepository _readuserrepository;
        private readonly IProfileWriteRepository _writeprofilerepository;
        private readonly IProfileReadRepository _readprofilerepository;

        public UserService(IUserWriteRepository userwriteRepository,
            IUserReadRepository userreadRepository,
            IProfileWriteRepository profilewriterepository,
            IProfileReadRepository profileReadRepository)
        {
            _writeuserrepository = userwriteRepository;
            _readuserrepository = userreadRepository;
            _readprofilerepository = profileReadRepository;
            _writeprofilerepository = profilewriterepository;
        }

        public async Task<OperationResult<UserResponse>> Create(UserRequest user)
        {
            try
            {
                IEnumerable<User> users = await _readuserrepository.GetAllAsync(x => x.Email == user.Email);
                var userExist = users.FirstOrDefault();

                if (userExist != null)
                {
                    return new OperationResult<UserResponse>
                    {
                        Success = false,
                        Message = "Já existe um usuário com este e-mail cadastrado.",
                        Data = null
                    };
                }

                User newUser = new User
                {
                    Email = user.Email,
                    Name = user.Name,
                    Password = user.Password,
                };

                await _writeuserrepository.Add(newUser);

                var response = new UserResponse
                {
                    Email = newUser.Email,
                    Name = newUser.Name,
                };

                return new OperationResult<UserResponse>
                {
                    Success = true,
                    Message = "Usuário cadastrado com sucesso.",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new OperationResult<UserResponse>
                {
                    Success = false,
                    Message = $"Erro ao cadastrar usuário: {ex.Message}",
                    Data = null
                };
            }
        }



        public async Task<OperationResult<ProfileResponse>> Profile(string email)
        {
            IEnumerable<User> users = await _readuserrepository.GetAllAsync(x => x.Email == email);
            var userExist = users.FirstOrDefault();

            if (userExist == null)
            {
                return new OperationResult<ProfileResponse>
                {
                    Success = false,
                    Message = "Usuário não encontrado",
                    Data = null
                };
            }

            var result = await _readprofilerepository.GetAllAsync(x => x.Id == userExist.Id );
            var getresult = result.FirstOrDefault();

            if (getresult == null)
            {
                return new OperationResult<ProfileResponse>
                {
                    Success = false,
                    Message = "Perfil não encontrado",
                    Data = null
                };
            }
            ProfileResponse response = new ProfileResponse
            {
                Name = userExist.Name,
                Email = userExist.Email,
                Salary = getresult.Salary
            };

            return new OperationResult<ProfileResponse>
            {
                Success = true,
                Message = "Perfil carregado com sucesso",
                Data = response
            };
        }

        public async Task<OperationResult<ProfileResponse>> ProfileSave(ProfileRequest profile)
        {
            var user = (await _readuserrepository.GetAllAsync(x => x.Email == profile.Email)).FirstOrDefault();

            if (user == null)
            {
                return new OperationResult<ProfileResponse>
                {
                    Success = false,
                    Message = "Usuário não encontrado",
                    Data = null
                };
            }

            Profile newProfile = new Profile
            {
                UserId = user.Id,
                Email = profile.Email,
                Salary = profile.Salary
            };

            await _writeprofilerepository.Add(newProfile);

            var response = new ProfileResponse
            {
                Email = profile.Email,
                Salary = profile.Salary,
                Name = user.Name
            };

            return new OperationResult<ProfileResponse>
            {
                Success = true,
                Message = "Perfil criado com sucesso!",
                Data = response
            };
        }

    }
}
