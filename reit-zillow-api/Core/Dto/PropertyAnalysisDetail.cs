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
            get { return Incomes == null ? 0 : Incomes.Sum(keyValue => keyValue.Value);  }
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

    }
}
