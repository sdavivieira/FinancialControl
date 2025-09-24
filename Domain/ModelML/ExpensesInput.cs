using FinancialControl.Domain.Models;
using Microsoft.ML.Data;

namespace FinancialControl.Domain.ModelML
{
    public class ExpensesInput
    {
        public string ExpenseType { get; set; }
        public float Value { get; set; }
        public int UserId { get;  set; }
    }
}
