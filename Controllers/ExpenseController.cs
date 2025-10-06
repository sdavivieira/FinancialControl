
using FinancialControl.Application.Interface;
using FinancialControl.ResponseRequest;
using FinancialControl.ResponseRequest.Request.Expense;
using FinancialControl.ResponseRequest.Response.Expense;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialControl.Controllers
{
    public class ExpenseController : BaseControllerapi
    {
        private readonly IExpenseService _expenseService;

        public ExpenseController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        [Authorize]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterExpense(ExpenseRequest expense)
        {
            var user = HttpContext.User;
            int userId = int.Parse((user.Claims.FirstOrDefault(c => c.Type == "id")?.Value!));
             
            var response = await _expenseService.Create(expense, userId);

            return Ok(response);
        }

        [Authorize]
        [HttpGet("expense")]
        public async Task<IActionResult> Expense()
        {
            OperationResult<IEnumerable<ExpenseResponse>> response = await _expenseService.Expenses();

            return Ok(response);
        }

        [Authorize]
        [HttpPut("expense/update/{id}")]
        public async Task<IActionResult> UpdateExpense(int id, ExpenseRequest expense)
        {
            var user = HttpContext.User;
            int userId = int.Parse((user.Claims.FirstOrDefault(c => c.Type == "id")?.Value!));

            var response = await _expenseService.Update(id, expense, userId);

            if (response.Success)
                return Ok(response);
            else
                return BadRequest(response);
        }

        [Authorize]
        [HttpDelete("expense/delete/{id}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            var user = HttpContext.User;
            int userId = int.Parse((user.Claims.FirstOrDefault(c => c.Type == "id")?.Value!));

            var response = await _expenseService.Delete(id, userId);

            if (response.Success)
                return Ok(response);
            else
                return BadRequest(response);
        }

    }
}
