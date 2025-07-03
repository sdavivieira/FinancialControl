using FinancialControl.Domain.Interfaces.Expenses;
using FinancialControl.Domain.Models;
using FinancialControl.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace FinancialControl.Infrastructure.Repositories
{
    public class ExpenseRepository : GenericRepository<Expense>,IExpenseReadRepository, IExpenseWriteRepository
    {
        private readonly ApplicationDbContext _context;
        public ExpenseRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;

        }

        public async Task<IEnumerable<Expense>> GetAllWithExpenseType()
        {
           return await _context.Expenses.Include(e => e.ExpenseType).ToListAsync();
        }
    }
}
