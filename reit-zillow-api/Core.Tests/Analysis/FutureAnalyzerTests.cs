using Core.Amortization;
using Core.Analyzer;
using Core.Dto;
using Core.Expense;
using Core.Loan;
using Core.Options;
using Core.PropertyValue;
using Core.Selling;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Tests.Analysis
{
    public class FutureAnalyzerTests
    {
        private readonly Mock<IPropertyValueEstimator> _mockPropertyValueEstimator;
        private readonly Mock<IAmortizationScheduleCalculator> _mockAmortizationScheduler;
        private readonly Mock<ISellingCostEstimator> _mockSellingCostEstimator;

        public FutureAnalyzerTests()
        {
            _mockPropertyValueEstimator = new Mock<IPropertyValueEstimator>();
            _mockAmortizationScheduler = new Mock<IAmortizationScheduleCalculator>();
            _mockSellingCostEstimator = new Mock<ISellingCostEstimator>();
        }

        [Fact]
        public void AnalyzeInvestment()
        {
            // arrange 


            const int HoldingPeriodInYears = 5;
            const double DownPaymentAmount = 60000;
            const double OriginalLoanAmount = 432200;
            const double InterestRate = 2.75;
            const double RemainingLoanAmount = 382479.27;
            const double OriginalPurchaseAmount = 570000;
            const double PropertyValueAtSell = 692408;  // assumed 4% appreciation per year. 
            const double AgentFeesAtSell = 41244.48;  // 6% of property value 
            double closingCostAtSell = 15000;
            double RepairsCost = 5000;

            const double TaxOnSell = 8424.53;  // 15% of ProfitOnSell

            const double SellingCosts = 69669.08; // DefaultClosingCostOnSell + Agent fees + TaxOnSell + DefaultRepaitCostOnSell

            const double NetProfit = 180259.72; // PropertyValueAtSell - SellingCosts

            const LoanProgram LoanProgram = LoanProgram.ThirtyYearFixed;

            _mockPropertyValueEstimator.Setup(estimator => estimator.EvaluatePropertyValue(OriginalPurchaseAmount, HoldingPeriodInYears)).Returns(PropertyValueAtSell);

            AmortizationScheduleEntry[] amortizationEntries = new AmortizationScheduleEntry[HoldingPeriodInYears * 12];
            amortizationEntries[amortizationEntries.Length - 1] = new AmortizationScheduleEntry()
            {
                RemainingBalance = RemainingLoanAmount
            };

            _mockAmortizationScheduler.Setup(calculator => calculator.Calculate(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<DateTime>(), LoanProgram)).Returns(amortizationEntries);

            _mockSellingCostEstimator.Setup(estimator => estimator.EstimateSellingCost(It.IsAny<double>(), It.IsAny<double>())).Returns(SellingCosts);

            IFutureAnalyzer analyzer = new FutureAnalyzer(_mockAmortizationScheduler.Object, _mockPropertyValueEstimator.Object,
                _mockSellingCostEstimator.Object);


            // act 

            double actualNetProfit = analyzer.CalculateNetProfitsOnSell(new FutureAnalyzerRequest()
            {
                DownPaymentAmount = DownPaymentAmount,
                OriginalLoanAmount = OriginalLoanAmount,
                OriginalPurchaseAmount = OriginalPurchaseAmount,
                HoldingPeriodInYears = 5,
                LoanProgram = LoanProgram
            });
            // assert 
            Assert.Equal(NetProfit, actualNetProfit, 0);
        }
    }
}
