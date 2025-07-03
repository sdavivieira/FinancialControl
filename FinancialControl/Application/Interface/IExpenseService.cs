using FinancialControl.ResponseRequest.Request.Expense;
using FinancialControl.ResponseRequest.Response.Expense;
using FinancialControl.ResponseRequest;

namespace FinancialControl.Application.Interface
{
    public interface IExpenseService
    {
        Task<OperationResult<ExpenseResponse>> Create(ExpenseRequest expense, string email);
        Task<OperationResult<IEnumerable<ExpenseResponse>>> Expenses();
    }
}
