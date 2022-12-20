using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto
{
    public record FutureAnalyzerResponse
    {
        public double TotalMoneyAfterHoldWithoutMonthlyCashflow { get; set; }
        public FutureAnalyzerRequest? AnalyzerInputs { get; set; }
        public FutureAnalyzerConfigs? AnalyzerConfigs { get; set; }
        public double MoneyMadePerMonth
        {
            get
            {
                return AnalyzerInputs == null || AnalyzerInputs.HoldingPeriodInYears == 0 ? 0 : TotalMoneyAfterHoldWithoutMonthlyCashflow / (AnalyzerInputs.HoldingPeriodInYears * 12);
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
