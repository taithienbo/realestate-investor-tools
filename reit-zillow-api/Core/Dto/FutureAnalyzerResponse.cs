using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto
{
    public record FutureAnalyzerResponse
    {
        public double TotalMoneyMadeAfterHold { get; set; }
        public FutureAnalyzerRequest? Inputs { get; set; }
        public double MoneyMadePerMonth
        {
            get
            {
                return Inputs == null || Inputs.HoldingPeriodInYears == 0 ? 0 : TotalMoneyMadeAfterHold / (Inputs.HoldingPeriodInYears * 12);
            }
        }
    }
}
