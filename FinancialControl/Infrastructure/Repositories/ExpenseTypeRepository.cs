using FinancialControl.Domain.Interfaces.Expenses;
using FinancialControl.Domain.Models;
using FinancialControl.Infrastructure.DbContext;

namespace FinancialControl.Infrastructure.Repositories
{
    public class ExpenseTypeRepository : GenericRepository<Expense>, IExpenseTypeReadRepository, IExpenseTypeWriteRepository
    {
        public ExpenseTypeRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
