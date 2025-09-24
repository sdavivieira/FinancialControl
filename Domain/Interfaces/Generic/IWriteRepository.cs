namespace FinancialControl.Domain.Interfaces.Generic
{
    public interface IWriteRepository<T>
    {
        Task Add(T entity);
        Task Delete(T entity);
        Task Update(T entity);

    }
}
