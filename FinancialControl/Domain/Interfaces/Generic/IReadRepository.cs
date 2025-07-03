using System.Linq.Expressions;

namespace FinancialControl.Domain.Interfaces.Generic
{
    public interface IReadRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null);

        Task<T> GetById(object id);
    }
}
