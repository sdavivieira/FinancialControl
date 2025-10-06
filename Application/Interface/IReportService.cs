namespace FinancialControl.Application.Interface
{
    public interface IReportService
    {
        Task<byte[]> GetExpensesReport();

    }
}
