using FinancialControl.Application.Interface;
using FinancialControl.Application.Service;
using FinancialControl.ResponseRequest;
using FinancialControl.ResponseRequest.Request.Expense;
using FinancialControl.ResponseRequest.Response.Expense;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialControl.Controllers
{
    public class ExpenseTypeController : BaseControllerapi
    {
        private readonly IExpenseTypeService _expenseTypeService;

        public ExpenseTypeController(IExpenseTypeService expenseTypeService)
        {
            _expenseTypeService = expenseTypeService;
        }

        [Authorize]
        [HttpPost("registertype")]
        public async Task<IActionResult>ExpenseType(ExpenseTypeRequest expense)
        {
            OperationResult<ExpenseTypeResponse> response = await _expenseTypeService.Create(expense);
            return Ok(response);
        }

        [Authorize]
        [HttpGet("types")]
        public async Task<IActionResult> ExpenseTypes()
        {
            OperationResult<IEnumerable<ExpenseTypeResponse>> response = await _expenseTypeService.ExpenseTypes();
            return Ok(response);
        }

        [Authorize]
        [HttpPut("expensetype/update/{id}")]
        public async Task<IActionResult> UpdateType(int id, ExpenseTypeRequest expense)
        {
            var response = await _expenseTypeService.Update(id, expense);
            if (response.Success)
                return Ok(response);
            else
                return BadRequest(response);
        }

        [Authorize]
        [HttpDelete("expensetype/delete/{id}")]
        public async Task<IActionResult> DeleteType(int id)
        {
            var response = await _expenseTypeService.Delete(id);
            if (response.Success)
                return Ok(response);
            else
                return BadRequest(response);
        }
    }
}
