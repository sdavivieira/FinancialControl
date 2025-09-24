namespace FinancialControl.Domain.Models
{
    public class ExpenseType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal InicialValue { get; set; }
        public bool IsFixed { get; set; }
    }
}
