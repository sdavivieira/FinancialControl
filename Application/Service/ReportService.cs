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

        public ReportService(
            IExpenseReadRepository expenseReadRepository,
            IExpenseTypeReadRepository expenseTypeReadRepository)
        {
            _expenseReadRepository = expenseReadRepository;
            _expenseTypeReadRepository = expenseTypeReadRepository;
        }

        public async Task<byte[]> GetExpensesReport()
        {
            // 1️⃣ Pegar dados do banco
            var expenses = await _expenseReadRepository.GetAllWithExpenseType();
            var expenseTypes = await _expenseTypeReadRepository.GetAllAsync();

            if (!expenses.Any() || !expenseTypes.Any())
                return Array.Empty<byte>();

            var typesDict = expenseTypes.ToDictionary(x => x.Name, x => x);

            // 2️⃣ Montar lista combinada para exportação
            var exportData = expenses.Select(e =>
            {
                typesDict.TryGetValue(e.ExpenseType.Name, out var type);
                return new
                {
                    Despesa = e.ExpenseType.Name,
                    Tipo = type?.Name ?? "",
                    ValorInicial = type?.InicialValue ?? 0m,
                    Valor = e.Value,
                    Fixa = type?.IsFixed ?? false
                };
            }).ToList();

            // 3️⃣ Criar DataTable
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

            // 4️⃣ Gerar Excel com ClosedXML
            using (var wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add(dt, "Despesas");

                // Cabeçalho em negrito
                ws.Row(1).Style.Font.Bold = true;

                // Ajustar largura automática
                ws.Columns().AdjustToContents();

                // Formatar valores monetários
                ws.Column("C").Style.NumberFormat.Format = "#,##0.00"; // Valor Inicial
                ws.Column("D").Style.NumberFormat.Format = "#,##0.00"; // Valor

                // Filtro automático
                //ws.RangeUsed().SetAutoFilter();

                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }
    }
}
