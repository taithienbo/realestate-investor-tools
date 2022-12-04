using Core.Amortization;
using Core.Dto;
using Core.Expense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Tests.Amortization
{
    public class AmortizationCalculatorTests
    {
        private readonly IAmortizationScheduleCalculator _amortizationScheduleCalculator;
        private readonly IMortgageExpenseEstimator _mortgageExpenseEstimator;

        public AmortizationCalculatorTests()
        {
            _mortgageExpenseEstimator = new MortgageExpenseEstimator();
            _amortizationScheduleCalculator = new AmortizationScheduleCalculator(_mortgageExpenseEstimator);
        }

        [Fact]
        public void LoanAmortizationSchedule()
        {
            // arrange 
            const double StartingPrincipal = 432200;
            const double InterestRate = 2.75;
            DateTime LoanStartDate = DateTime.Parse("2020-10-01");

            // act 
            AmortizationScheduleEntry[] schedule = _amortizationScheduleCalculator.Calculate(StartingPrincipal, InterestRate, LoanStartDate);

            // assert 
            Assert.NotNull(schedule);
            Assert.NotEmpty(schedule);

            const double ExpectedFirstMonthInterest = 990.458;
            const double ExpectedFirstMonthPrincipal = 773.962;
            const double ExpectedFirstMonth = 10;
            const double ExpectedFirstYear = 2020;
            AmortizationScheduleEntry firstEntry = schedule[0];
            Assert.Equal(ExpectedFirstMonthInterest, firstEntry.Interest, 0);
            Assert.Equal(ExpectedFirstMonthPrincipal, firstEntry.Principal, 0);
            Assert.Equal(ExpectedFirstYear, firstEntry.Year);
            Assert.Equal(ExpectedFirstMonth, firstEntry.Month);

            Assert.True(schedule.Length > 1);
            const double ExpectedSecondMonthInterest = 988.68;
            const double ExpectedSecondMonthPrincipal = 775.73;
            const double ExpectedSecondMonth = 11;
            const double ExpectedSecondYear = 2020;
            AmortizationScheduleEntry secondEntry = schedule[1];
            Assert.Equal(ExpectedSecondMonthInterest, secondEntry.Interest, 0);
            Assert.Equal(ExpectedSecondMonthPrincipal, secondEntry.Principal, 0);
            Assert.Equal(ExpectedSecondYear, secondEntry.Year);
            Assert.Equal(ExpectedSecondMonth, secondEntry.Month);

            const double ExpectedNumOfPayments = 360; // 30 years * 12 months; 
            Assert.Equal(ExpectedNumOfPayments, schedule.Length);
            const double ExpectedLastMonthInterest = 4.03;
            const double ExpectedLastMonthPrincipal = 1760.38;
            const double ExpectedLastYear = 2050;
            const double ExpectedLastMonth = 9; // september
            AmortizationScheduleEntry lastEntry = schedule[schedule.Length - 1];
            Assert.Equal(ExpectedLastMonthInterest, lastEntry.Interest, 0);
            Assert.Equal(ExpectedLastMonthPrincipal, lastEntry.Principal, 0);
            Assert.Equal(ExpectedLastYear, lastEntry.Year);
            Assert.Equal(ExpectedLastMonth, lastEntry.Month);
        }
    }
}
