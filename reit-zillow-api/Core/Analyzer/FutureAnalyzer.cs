using Core.Amortization;
using Core.Dto;
using Core.Options;
using Core.PropertyValue;
using Core.Selling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Analyzer
{
    public class FutureAnalyzer : IFutureAnalyzer
    {
        private readonly IAmortizationScheduleCalculator _amortizationScheduleCalculator;
        private readonly ISellingCostEstimator _sellingCostEstimator;
        private readonly IPropertyValueEstimator _propertyValueEstimator;


        public FutureAnalyzer(IAmortizationScheduleCalculator amortizationScheduleCalculator,
            IPropertyValueEstimator propertyValueEstimator,
            ISellingCostEstimator sellingCostEstimator)
        {
            _amortizationScheduleCalculator = amortizationScheduleCalculator;
            _propertyValueEstimator = propertyValueEstimator;
            _propertyValueEstimator = propertyValueEstimator;
            _sellingCostEstimator = sellingCostEstimator;
        }

        public double CalculateNetProfitsOnSell(FutureAnalyzerRequest parameters)
        {
            AmortizationScheduleEntry[] amortizationSchedule = _amortizationScheduleCalculator.Calculate(parameters.OriginalLoanAmount, parameters.InterestRate, DateTime.Now.Date, parameters.LoanProgram);

            int holdingPeriodInMonths = parameters.HoldingPeriodInYears * 12;

            double remainingBalanceAfterHold = amortizationSchedule[holdingPeriodInMonths - 1].RemainingBalance;

            double totalSell = _propertyValueEstimator.EvaluatePropertyValue(parameters.OriginalPurchaseAmount, parameters.HoldingPeriodInYears);

            double sellingCosts = _sellingCostEstimator.EstimateSellingCost(parameters.OriginalPurchaseAmount, totalSell);

            double totalCosts = parameters.DownPaymentAmount + sellingCosts + remainingBalanceAfterHold;


            double netProfit = totalSell - totalCosts;

            return netProfit;
        }
    }
}
