namespace FinancialControl.ResponseRequest.Response.Expense
{
    public class ExpenseTypeResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal InicialValue { get; set; }
        public bool IsFixed { get; internal set; }
    }
}
