using FinancialControl.Domain.Models;

namespace FinancialControl.ResponseRequest.Request.Expense
{
    public class ExpenseRequest
    {
        public int ExpenseTypeId { get; set; }
        public decimal Value { get; set; }
    }
}
