namespace FinancialControl.Domain.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public int ExpenseTypeId { get; set; }
        public decimal Value { get; set; }
        public DateTime? Date { get; set; }
        public ExpenseType ExpenseType { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
