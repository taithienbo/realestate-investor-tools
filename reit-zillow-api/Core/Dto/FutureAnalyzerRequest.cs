using Core.Loan;


namespace Core.Dto
{
    public record FutureAnalyzerRequest
    {
        public double EstimatedMonthlyCashflow { get; set; }
        public double DownPaymentAmount { get; set; }
        public double OriginalLoanAmount { get; set; }
        public double OriginalPurchaseAmount { get; set; }
        public int HoldingPeriodInYears { get; set; }
        public double InterestRate { get; set; }
        public LoanProgram LoanProgram { get; set; }
    }
}
