using FinancialControl.ResponseRequest.Request.Expense;
using FinancialControl.ResponseRequest.Response.Expense;
using FinancialControl.ResponseRequest;

namespace FinancialControl.Application.Interface
{
    public interface IExpenseService
    {
        Task<OperationResult<ExpenseResponse>> Create(ExpenseRequest expense, int userId);
        Task<OperationResult<IEnumerable<ExpenseResponse>>> Expenses();
        Task<OperationResult<bool>> Delete(int id, int userId);
        Task<OperationResult<ExpenseResponse>> Update(int id, ExpenseRequest expense, int userId);

    }
}
