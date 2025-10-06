using FinancialControl.Domain.Interfaces.Expenses;
using ClosedXML.Excel;
using System.Data;
using FinancialControl.Application.Interface;

namespace FinancialControl.Application.Service
{
    public class ReportService : IReportService
    {
        private readonly IExpenseReadRepository _expenseReadRepository;
        private readonly IExpenseTypeReadRepository _expenseTypeReadRepository;
        private readonly IExpenseWriteRepository _expenseWriteRepository;
        private readonly IExpenseTypeWriteRepository _expenseTypeWriteRepository;

        public ReportService(
            IExpenseReadRepository expenseReadRepository,
            IExpenseTypeReadRepository expenseTypeReadRepository,
            IExpenseTypeWriteRepository expenseTypeWriteRepository,
            IExpenseWriteRepository expenseWriteRepository)
        {
            _expenseReadRepository = expenseReadRepository;
            _expenseTypeReadRepository = expenseTypeReadRepository;
            _expenseWriteRepository = expenseWriteRepository;
            _expenseTypeWriteRepository = expenseTypeWriteRepository;
        }

        public async Task<byte[]> GetExpensesReport()
        {
            var expenses = await _expenseReadRepository.GetAllWithExpenseType();
            var expenseTypes = await _expenseTypeReadRepository.GetAllAsync();

            if (!expenses.Any() || !expenseTypes.Any())
                return Array.Empty<byte>();

            var typesDict = expenseTypes.ToDictionary(x => x.Name, x => x);

            var exportData = expenses.Select(e =>
            {
                typesDict.TryGetValue(e.ExpenseType.Name, out var type);
                return new
                {
                    Despesa = e.ExpenseType.Name,
                    Tipo = type?.Name ?? "",
                    ValorInicial = type?.InicialValue ?? 0m,
                    Valor = e.Value,
                    Fixa = type?.IsFixed ?? false,
                    ExpenseEntity = e,
                    ExpenseTypeEntity = type
                };
            }).ToList();

            var dt = new DataTable();
            dt.Columns.Add("Despesa", typeof(string));
            dt.Columns.Add("Tipo", typeof(string));
            dt.Columns.Add("Valor Inicial", typeof(decimal));
            dt.Columns.Add("Valor", typeof(decimal));
            dt.Columns.Add("Fixa?", typeof(string));

            foreach (var item in exportData)
            {
                dt.Rows.Add(
                    item.Despesa,
                    item.Tipo,
                    item.ValorInicial,
                    item.Valor,
                    item.Fixa ? "Sim" : "Não"
                );
            }

            // 4️⃣ Deletar todos os Expenses
            foreach (var exp in exportData.Select(x => x.ExpenseEntity))
            {
                await _expenseWriteRepository.Delete(exp);
            }

            // 5️⃣ Deletar ExpenseTypes que não são fixos
            var typesToDelete = exportData
                .Select(x => x.ExpenseTypeEntity)
                .Where(x => x != null && !x.IsFixed)
                .Distinct()
                .ToList();

            foreach (var type in typesToDelete)
            {
                await _expenseTypeWriteRepository.Delete(type);
            }


            using (var wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add(dt, "Despesas");
                ws.Row(1).Style.Font.Bold = true;
                ws.Columns().AdjustToContents();
                ws.Column("C").Style.NumberFormat.Format = "#,##0.00";
                ws.Column("D").Style.NumberFormat.Format = "#,##0.00";

                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }
    }
}
