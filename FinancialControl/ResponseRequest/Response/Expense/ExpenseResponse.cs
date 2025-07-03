namespace FinancialControl.ResponseRequest.Response.Expense
{
    public class ExpenseResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal InicialValue { get; set; }
        public decimal Value { get; set; }
    }
}
