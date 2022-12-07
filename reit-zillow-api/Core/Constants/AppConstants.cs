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



    public enum CommonExpenseType
    {
        Mortgage,
        PropertyTax,
        HomeOwnerInsurance,
        CapitalExpenditures,
        Repairs,
        PropertyManagement,
        HoaFee,
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
}
