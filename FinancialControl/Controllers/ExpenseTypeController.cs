using FinancialControl.Application.Interface;
using FinancialControl.ResponseRequest;
using FinancialControl.ResponseRequest.Request.Expense;
using FinancialControl.ResponseRequest.Response.Expense;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialControl.Controllers
{
    public class ExpenseTypeController : BaseControllerapi
    {
        private readonly IExpenseService _expenseService;

        public ExpenseTypeController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        [Authorize]
        [HttpPost("register")]
        public async Task<IActionResult>ExpenseType(ExpenseTypeRequest expense)
        {
            OperationResult<ExpenseTypeResponse> response = await _expenseService.Create(expense);
            return Ok(response);
        }
    }
}
