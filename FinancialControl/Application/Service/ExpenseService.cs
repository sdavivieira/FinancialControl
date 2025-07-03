using FinancialControl.Application.Interface;
using FinancialControl.Domain.Interfaces.Expenses;
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
        public ExpenseService(IExpenseWriteRepository expenseWriteRepository, 
            IExpenseReadRepository expenseReadRepository)
        {
            _expenseWriteRepository = expenseWriteRepository;
            _expenseReadRepository = expenseReadRepository;
        }
        public async Task<OperationResult<ExpenseResponse>> Create(ExpenseRequest expense)
        {
            try
            {

                var newexpense = new Expense()
                {
                    Date = expense.Date,
                    ExpenseTypeId = expense.ExpenseTypeId,

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
