using Core.Calculators;
using Core.Constants;
using Infrastructure.Calculator;

namespace Infrastructure.Tests.Calculators
{
    public class MortgageCalculatorTests
    {
        private IMortgageCalculator _mortgageCalculator;

        public MortgageCalculatorTests()
        {
            _mortgageCalculator = new MortgageCalculator();
        }

        [Fact]
        public void CalculateMortgate()
        {
            // arrange 
            var mortgagePrincipal = 400000;
            var annualInterestRate = 6.746;
            var loanProgram = LoanProgram.ThirtyYearFixed;
            var expectedMonthlyMortagePrincipalAndInterest = 2593;
            // act 
            var actualMonthlyMortgagePrincipalAndInterest = _mortgageCalculator.Calculate(mortgagePrincipal, annualInterestRate, loanProgram);
            // assert 
            Assert.Equal(expectedMonthlyMortagePrincipalAndInterest, actualMonthlyMortgagePrincipalAndInterest, 0);
        }

        [Fact]
        public void CalculateMortgage_Test2_UseDefaultLoanProgram()
        {
            // arrange 
            var mortgagePrincipal = 454500;
            var annualInterestRate = 7.246;
            var expectedMonthlyMortgagePrincipalAndInterest = 3099;
            // act 
            var actualMonthlyMortgagePrincipalAndInterest = _mortgageCalculator.Calculate(mortgagePrincipal, annualInterestRate);
            // assert
            Assert.Equal(expectedMonthlyMortgagePrincipalAndInterest, actualMonthlyMortgagePrincipalAndInterest, 0);

        }
    }
}
