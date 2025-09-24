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
        private readonly IExpenseTypeService _expenseService;

        public ExpenseTypeController(IExpenseTypeService expenseService)
        {
            _expenseService = expenseService;
        }

        [Authorize]
        [HttpPost("registertype")]
        public async Task<IActionResult>ExpenseType(ExpenseTypeRequest expense)
        {
            OperationResult<ExpenseTypeResponse> response = await _expenseService.Create(expense);
            return Ok(response);
        }

        [Authorize]
        [HttpGet("types")]
        public async Task<IActionResult> ExpenseTypes()
        {
            OperationResult<IEnumerable<ExpenseTypeResponse>> response = await _expenseService.ExpenseTypes();
            return Ok(response);
        }

    }
}
