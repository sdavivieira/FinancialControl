using FinancialControl.Application.Interface;
using FinancialControl.Domain.Interfaces.Expenses;
using FinancialControl.Domain.Models;
using FinancialControl.ResponseRequest;
using FinancialControl.ResponseRequest.Request.Expense;
using FinancialControl.ResponseRequest.Response.Expense;

namespace FinancialControl.Application.Service
{
    public class ExpenseTypeService : IExpenseTypeService
    {
        private readonly IExpenseTypeWriteRepository _expenseTypeWriteRepository;
        private readonly IExpenseTypeReadRepository _expenseTypeReadRepository;

        public ExpenseTypeService(IExpenseTypeWriteRepository expenseWriteRepository,
            IExpenseTypeReadRepository expenseReadRepository)
        {
            _expenseTypeWriteRepository = expenseWriteRepository;
            _expenseTypeReadRepository = expenseReadRepository;
        }
        public async Task<OperationResult<ExpenseTypeResponse>> Create(ExpenseTypeRequest expense)
        {
            try
            {

                ExpenseType newexpense = new ExpenseType
                {
                    Name = expense.Name,
                    InicialValue = expense.InicialValue,
                    IsFixed = expense.IsFixed,
                };

                await _expenseTypeWriteRepository.Add(newexpense);

                var response = new ExpenseTypeResponse
                {
                    Name = expense.Name,
                    InicialValue = expense.InicialValue,
                    IsFixed = expense.IsFixed
                };

                return new OperationResult<ExpenseTypeResponse>
                {
                    Success = true,
                    Message = "Novo registro cadastrado",
                    Data = response
                };
            }
                catch (Exception ex)
                {
                    return new OperationResult<ExpenseTypeResponse>()
                    {
                        Success = false,
                        Message = "Erro ao realizar Registro!",
                        Data = null
                    };
                }

            }

        public async Task<OperationResult<IEnumerable<ExpenseTypeResponse>>> ExpenseTypes()
        {
            IEnumerable<ExpenseType> result =  await _expenseTypeReadRepository.GetAllAsync();

            if (!result.Any())
            {
                return new OperationResult<IEnumerable<ExpenseTypeResponse>>
                {
                    Success = false,
                    Message = "Não Encontrado Registro",
                    Data =  new List<ExpenseTypeResponse>() 
                };
            }

            IEnumerable<ExpenseTypeResponse> response = result.Select(x => new ExpenseTypeResponse
            {
                Id = x.Id,
                Name = x.Name,
                InicialValue = x.InicialValue,
            });

            return new OperationResult<IEnumerable<ExpenseTypeResponse>>
            {
                Success = true,
                Message = "Encontrado Registros",
                Data = response
            };
        }
    }
}
