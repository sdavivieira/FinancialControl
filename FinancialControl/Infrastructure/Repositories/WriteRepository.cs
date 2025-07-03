using FinancialControl.Domain.Interfaces.Generic;
using FinancialControl.Infrastructure.DbContext;

namespace FinancialControl.Infrastructure.Repositories
{
    public class WriteRepository<T> : IWriteRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;

        public WriteRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Add(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync(); 
        }

        public async Task Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Update(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
