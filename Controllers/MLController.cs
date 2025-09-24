using FinancialControl.Domain.Models;
using FinancialControl.ML;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialControl.Controllers
{
    public class MLController : BaseControllerapi
    {
        private readonly ExpenseModelTrainer _trainer;

        public MLController(ExpenseModelTrainer trainer)
        {
            _trainer = trainer;
        }


        [Authorize]
        [HttpPost("train/{userId}")]
        public async Task<IActionResult> Train(int userId)
        {
            IEnumerable<Expense> expenses = [];
            string NamePath = "Path";
            await _trainer.LoadingDataAsync(userId, expenses);
            await _trainer.TrainingModel();
            await _trainer.SaveModel(NamePath);

            return Ok("Modelo treinado com sucesso");
        }
    }
}
