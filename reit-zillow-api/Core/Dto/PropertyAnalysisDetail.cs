using Core.Constants;

namespace Core.Dto
{
    public class PropertyAnalysisDetail
    {
        public IDictionary<string, double>? Incomes { get; set; }
        public IDictionary<string, double>? Expenses { get; set; }
        public double TotalExpense
        {
            get { return Expenses == null ? 0 : Expenses.Sum(keyValue => keyValue.Value); }
        }

        public double TotalIncome
        {
            get { return Incomes == null ? 0 : Incomes.Sum(keyValue => keyValue.Value); }
        }

        public double InterestRate { get; set; }
        public ListingDetail? ListingDetail { get; set; }

        public void AddIncome(string incomeType, double incomeAmount)
        {
            if (Incomes == null)
            {
                Incomes = new Dictionary<string, double>();
            }
            Incomes.Add(incomeType, incomeAmount);
        }

        public double NetOperatingIncome { get; set; }
        public double CapRate { get; set; }
        public double DebtServiceCoverageRatio { get; set; }
        public double CashOnCashReturn { get; set; }
        public double CashFlow { get; set; }
        public double AssumedDownPaymentPercent { get; set; }
        public double AssumedClosingCost { get; set; }
        public IDictionary<string, double> AssumedOutOfPocketCosts { get; set; } = new Dictionary<string, double>();

    }
}
