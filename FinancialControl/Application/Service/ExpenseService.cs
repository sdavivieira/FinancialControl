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
        private readonly IExpenseTypeWriteRepository _expenseWriteRepository;

        public ExpenseService(IExpenseTypeWriteRepository expenseWriteRepository)
        {
            _expenseWriteRepository = expenseWriteRepository;
        }
        public async Task<OperationResult<ExpenseTypeResponse>> Create(ExpenseTypeRequest expense)
        {
            if (expense.Name == null)
            {
                
            }

            Expense newexpense = new Expense
            {
                Name = expense.Name
            };
            
            await _expenseWriteRepository.Add(newexpense);


            var response = new ExpenseTypeResponse
            {
                Name = expense.Name
            };

            return new OperationResult<ExpenseTypeResponse>
            {
                Success = true,
                Message = "Novo registro cadastrado",
                Data = response
            };


        }
    }
}
