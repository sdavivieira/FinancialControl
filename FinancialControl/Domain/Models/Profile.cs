namespace FinancialControl.Domain.Models
{
    public class Profile
    {
        public int Id { get; set; }
        public decimal Salary { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Email { get; set; }
    }
}
