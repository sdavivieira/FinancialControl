using FinancialControl.Application.Interface;
using FinancialControl.Domain.Interfaces.Expenses;
using FinancialControl.Domain.Interfaces.Users;
using FinancialControl.Domain.Models;
using FinancialControl.ResponseRequest;
using FinancialControl.ResponseRequest.Request.Expense;
using FinancialControl.ResponseRequest.Response.Expense;

namespace FinancialControl.Application.Service
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseWriteRepository _expenseWriteRepository;
        private readonly IExpenseReadRepository _expenseReadRepository;
        private readonly IUserReadRepository _userReadRepository;
        public ExpenseService(IExpenseWriteRepository expenseWriteRepository, 
            IExpenseReadRepository expenseReadRepository,
            IUserReadRepository userReadRepository)
        {
            _expenseWriteRepository = expenseWriteRepository;
            _expenseReadRepository = expenseReadRepository;
            _userReadRepository = userReadRepository;
        }
        public async Task<OperationResult<ExpenseResponse>> Create(ExpenseRequest expense, int userId)
        {
            try
            {
                IEnumerable<User> users = await _userReadRepository.GetAllAsync(x => x.Id == userId);
                var userExist = users.FirstOrDefault();

                var newexpense = new Expense()
                {
                    Date = expense.Date,
                    ExpenseTypeId = expense.ExpenseTypeId,
                    UserId = userExist.Id
                };

                await _expenseWriteRepository.Add(newexpense);

                return new OperationResult<ExpenseResponse>()
                {
                    Success = true,
                    Message = "Registrado com sucesso!",
                    Data = null
                };
            }
            catch (Exception ex) {
                
                return new OperationResult<ExpenseResponse>()
                {
                    Success = false,
                    Message = "Erro ao realizar registro",
                    Data = null
                };
            }
        }

        public async Task<OperationResult<IEnumerable<ExpenseResponse>>> Expenses()
        {
              IEnumerable<Expense> result = await _expenseReadRepository.GetAllWithExpenseType();

                if (!result.Any())
                {
                    return new OperationResult<IEnumerable<ExpenseResponse>>
                    {
                        Success = false,
                        Message = "Não Encontrado Registro",
                        Data = new List<ExpenseResponse>()
                    };
                }

                IEnumerable<ExpenseResponse> response = result.Select(x => new ExpenseResponse
                {
                    Id = x.Id,
                    Name = x.ExpenseType.Name,
                    InicialValue = x.ExpenseType.InicialValue,
                    Value = x.Value
                });

                return new OperationResult<IEnumerable<ExpenseResponse>>
                {
                    Success = true,
                    Message = "Encontrado Registros",
                    Data = response
                };
            
        }
    }
}
