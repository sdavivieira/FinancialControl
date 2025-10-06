using FinancialControl.ResponseRequest;
using FinancialControl.ResponseRequest.Request.Expense;
using FinancialControl.ResponseRequest.Response.Expense;

namespace FinancialControl.Application.Interface
{
    public interface IExpenseTypeService
    {
        Task<OperationResult<ExpenseTypeResponse>> Create(ExpenseTypeRequest expense);
        Task<OperationResult<IEnumerable<ExpenseTypeResponse>>> ExpenseTypes();
        Task<OperationResult<ExpenseTypeResponse>> Update(int id, ExpenseTypeRequest expense);
        Task<OperationResult<bool>> Delete(int id);

    }
}
