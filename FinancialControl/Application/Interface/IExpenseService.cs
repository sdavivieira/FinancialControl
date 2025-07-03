using FinancialControl.ResponseRequest;
using FinancialControl.ResponseRequest.Request.Expense;
using FinancialControl.ResponseRequest.Response.Expense;

namespace FinancialControl.Application.Interface
{
    public interface IExpenseService
    {
        Task<OperationResult<ExpenseTypeResponse>> Create(ExpenseTypeRequest expense);
    }
}
