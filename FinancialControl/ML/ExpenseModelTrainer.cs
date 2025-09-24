using FinancialControl.Domain.ModelML;
using Microsoft.ML;
using FinancialControl.Domain.Models;
using System.Data;

namespace FinancialControl.ML
{
    public class ExpenseModelTrainer
    {
        private readonly MLContext mlContext = new MLContext();
        private IDataView dataView;
        private ITransformer modelTrained;

        public async Task LoadingDataAsync(int userId, IEnumerable<Expense> expenses)
        {

            var expenseInputs = expenses.Select(e => new ExpensesInput
            {
                ExpenseType = e.ExpenseType.Name,
                Value = (float)e.Value,
                UserId = e.UserId
            }).ToList();

            var userExpenses = expenseInputs.Where(e => e.UserId == userId).ToList();

            dataView = mlContext.Data.LoadFromEnumerable(userExpenses);

        }

        public async Task TrainingModel()
        {
            var pipeline = mlContext.Transforms
                .Categorical.OneHotEncoding(outputColumnName: "ExpenseTypeEncoded", inputColumnName: nameof(ExpensesInput.ExpenseType))
                .Append(mlContext.Transforms.Concatenate("Features", nameof(ExpensesInput.Value), "ExpenseTypeEncoded"))
                .Append(mlContext.Regression.Trainers.Sdca(
                    labelColumnName: "Value",
                    maximumNumberOfIterations: 100
                ));

            modelTrained = pipeline.Fit(dataView);

        }

        public async Task SaveModel(string modelPath)
        {
            var dir = Path.GetDirectoryName(modelPath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            mlContext.Model.Save(modelTrained, dataView.Schema, modelPath);
        }

    }
}
