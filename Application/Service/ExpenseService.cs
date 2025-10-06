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
                    Date = DateTime.UtcNow,
                    ExpenseTypeId = expense.ExpenseTypeId,
                    UserId = userExist.Id,
                    Value = expense.Value,
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
        public async Task<OperationResult<ExpenseResponse>> Update(int id, ExpenseRequest expense, int userId)
        {
            try
            {
                var existingExpense = (await _expenseReadRepository.GetAllAsync(x => x.Id == id && x.UserId == userId)).FirstOrDefault();
                if (existingExpense == null)
                    return new OperationResult<ExpenseResponse> { Success = false, Message = "Despesa não encontrada." };

                existingExpense.Value = expense.Value;
                existingExpense.ExpenseTypeId = expense.ExpenseTypeId;

                await _expenseWriteRepository.Update(existingExpense);

                return new OperationResult<ExpenseResponse> { Success = true, Message = "Despesa atualizada com sucesso." };
            }
            catch
            {
                return new OperationResult<ExpenseResponse> { Success = false, Message = "Erro ao atualizar despesa." };
            }
        }

        public async Task<OperationResult<bool>> Delete(int id, int userId)
        {
            try
            {
                var existingExpense = (await _expenseReadRepository.GetAllAsync(x => x.Id == id && x.UserId == userId)).FirstOrDefault();
                if (existingExpense == null)
                    return new OperationResult<bool> { Success = false, Message = "Despesa não encontrada.", Data = false };

                await _expenseWriteRepository.Delete(existingExpense);

                return new OperationResult<bool> { Success = true, Message = "Despesa deletada com sucesso.", Data = true };
            }
            catch
            {
                return new OperationResult<bool> { Success = false, Message = "Erro ao deletar despesa.", Data = false };
            }
        }
    }
}
