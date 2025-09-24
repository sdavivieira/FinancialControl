using FinancialControl.Domain.Interfaces.Generic;
using FinancialControl.Infrastructure.DbContext;
using System.Linq.Expressions;

namespace FinancialControl.Infrastructure.Repositories
{
    public class GenericRepository<T> : IWriteRepository<T>, IReadRepository<T> where T : class
    {
        private readonly ReadRepository<T> _readRepository;
        private readonly WriteRepository<T> _writeRepository;

        public GenericRepository(ApplicationDbContext context)
        {
            _readRepository = new ReadRepository<T>(context);
            _writeRepository = new WriteRepository<T>(context);
        }

        public async Task<T> GetById(object id)
        {
            return await _readRepository.GetById(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null)
        {
            return await _readRepository.GetAllAsync(predicate);
        }

        public async Task Add(T entity)
        {
            await _writeRepository.Add(entity);
        }

        public async Task Delete(T entity)
        {
            await _writeRepository.Delete(entity);
        }

        public async Task Update(T entity)
        {
            await _writeRepository.Update(entity);
        }

    }
}
