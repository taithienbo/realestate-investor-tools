using Core.Amortization;
using Core.Dto;
using Core.Options;
using Core.PropertyValue;
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
        private readonly AppOptions _appOptions;
        private readonly IPropertyValueEstimator _propertyValueEstimator;


        public FutureAnalyzer(IAmortizationScheduleCalculator amortizationScheduleCalculator,
            IPropertyValueEstimator propertyValueEstimator,
            AppOptions appOptions)
        {
            _amortizationScheduleCalculator = amortizationScheduleCalculator;
            _propertyValueEstimator = propertyValueEstimator;
            _propertyValueEstimator = propertyValueEstimator;
            _appOptions = appOptions;
        }

        public double CalculateNetProfitsOnSell(InvestmentOnSellAnalyzerParams parameters)
        {
            AmortizationScheduleEntry[] amortizationSchedule = _amortizationScheduleCalculator.Calculate(parameters.OriginalLoanAmount, parameters.InterestRate, DateTime.Now.Date, parameters.LoanProgram);

            int holdingPeriodInMonths = parameters.HoldingPeriodInYears * 12;

            double remainingBalanceAfterHold = amortizationSchedule[holdingPeriodInMonths - 1].RemainingBalance;
            double principalPaidDown = parameters.OriginalLoanAmount - remainingBalanceAfterHold;
            double propertyValueAfterHold = _propertyValueEstimator.EvaluatePropertyValue(parameters.OriginalPurchaseAmount, parameters.HoldingPeriodInYears);
            double agentFeesOnSell = _appOptions.DefaultAgentFeesPercentageOfSellingPrice / 100 * propertyValueAfterHold;
            double profitOnSell = propertyValueAfterHold - parameters.OriginalPurchaseAmount - agentFeesOnSell - _appOptions.DefaultClosingCostOnSell - _appOptions.DefaultRepairCostOnSell;
            double taxPaidOnSell = _appOptions.DefaultTaxPercentageOnSell / 100 * profitOnSell;
            double remainingAfterExpensesAndTaxes = profitOnSell - taxPaidOnSell;
            double netProfit = principalPaidDown + remainingAfterExpensesAndTaxes - parameters.DownPaymentAmount;

            return netProfit;
        }
    }
}
