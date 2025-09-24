using FinancialControl.Domain.Interfaces.Generic;
using FinancialControl.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FinancialControl.Infrastructure.Repositories
{
    public class ReadRepository<T> : IReadRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;

        public ReadRepository(ApplicationDbContext context)
        {
            _context = context;      
        }
        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null)
        {
            if (predicate == null)
                return await _context.Set<T>().AsNoTracking().ToListAsync();

            return await _context.Set<T>().AsNoTracking().Where(predicate).ToListAsync();
        }



        public async Task<T> GetById(object id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

    }
}
