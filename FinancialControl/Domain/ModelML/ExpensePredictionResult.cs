using Microsoft.ML.Data;

namespace FinancialControl.Domain.ModelML
{
    public class ExpensePredictionResult
    {

        [ColumnName("Score")]
        public float PredictionValue { get; set; }
    }
}
