namespace FinancialControl.ResponseRequest.Request.Expense
{
    public class ExpenseTypeRequest
    {
        public string Name { get; set; }
        public decimal InicialValue { get; set; }
        public bool IsFixed { get; set; }
    }
}
