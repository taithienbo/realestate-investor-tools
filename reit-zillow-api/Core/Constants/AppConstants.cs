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
}
