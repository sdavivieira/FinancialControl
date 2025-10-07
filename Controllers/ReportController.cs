using FinancialControl.Application.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinancialControl.Controllers
{
    public class ReportController : BaseControllerapi
    {
        private readonly IReportService _reportService;
        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("ExportExcel")]
        public async Task<IActionResult> ExportExcel()
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var fileBytes = await _reportService.GetExpensesReport(userId);

            if (fileBytes.Length == 0)
                return NotFound("Não há dados para exportar.");

            return File(
                fileBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Relatorio.xlsx"
            );
        }

    }
}
