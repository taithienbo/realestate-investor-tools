using Core.Constants;
using Core.Loan;

namespace Core.Dto
{
    public class LoanDetail
    {
        public double DownPaymentPercent { get; set; }
        public double InterestRate { get; set; }
        public LoanProgram LoanProgram { get; set; }
    }
}
