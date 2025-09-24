using FinancialControl.Domain.Interfaces.Generic;
using FinancialControl.Domain.Models;

namespace FinancialControl.Domain.Interfaces.Expenses
{
    public interface IExpenseReadRepository : IReadRepository<Expense>
    {
        Task<IEnumerable<Expense>> GetAllWithExpenseType();

    }
}
