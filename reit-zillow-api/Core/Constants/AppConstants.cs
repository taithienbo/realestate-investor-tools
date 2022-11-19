namespace Core.Constants
{

    public static class PropertyCondition
    {
        public const string TurnKey = nameof(TurnKey);
        public const string Fixer = nameof(Fixer);
    }

    public static class HomeType
    {
        public const string SingleFamily = nameof(SingleFamily);
        public const string Condo = nameof(Condo);
    }

    public enum LoanProgram
    {
        ThirtyYearFixed
    }

    public enum CommonExpenseType
    {
        Mortgage,
        PropertyTax,
        HomeOwnerInsurance,
        CapitalExpenditures,
        Repairs,
        PropertyManagement,
        Misc
    }

    public enum CommonIncomeType
    {
        Rental
    }

    public enum CommonOutOfPocketCost
    {
        DownPayment,
        ClosingCost
    }

    public class OutOfPocketInvestmentCost
    {
        public const double DefaultDownPaymentPercent = 25;
        public const double DefaultClosingCostAmount = 15000;
    }
}
