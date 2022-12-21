using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto
{
    public record FutureAnalyzerResponse
    {
        public double TotalAmountAfterHoldWithoutCashFlow { get; set; }
        public double TotalAmountAfterHoldWithCashFlow
        {
            get
            {
                return (EstimatedMonthlyCashflow * (AnalyzerInputs?.HoldingPeriodInYears ?? 0) * 12) + TotalAmountAfterHoldWithoutCashFlow;
            }
        }
        public double EstimatedMonthlyCashflow { get; set; }
        public FutureAnalyzerRequest? AnalyzerInputs { get; set; }
        public FutureAnalyzerConfigs? AnalyzerConfigs { get; set; }
        public double AmountPerMonthWithoutCashFlow
        {
            get
            {
                return AnalyzerInputs == null || AnalyzerInputs.HoldingPeriodInYears == 0 ? 0 : TotalAmountAfterHoldWithoutCashFlow / (AnalyzerInputs.HoldingPeriodInYears * 12);
            }
        }
        public double AmountPerMonthWithCashFlow
        {
            get
            {
                return AmountPerMonthWithoutCashFlow + EstimatedMonthlyCashflow;
            }
        }
    }

    public record FutureAnalyzerConfigs
    {
        public double EstimatedClosingCostOnSell { get; set; }
        public double EstimatedRepairCostOnSell { get; set; }
        public double EstimatedAgentFeesPercentageOfSellingPrice { get; set; }
        public double DownPaymentPercentage { get; set; }
        public double EstimatedYearlyIncreaseInPropertyValue { get; set; }
    }
}
