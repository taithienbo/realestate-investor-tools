using Core.Loan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Options
{
    public record AppOptions
    {
        public const string App = nameof(App);

        public double DefaultDownPaymentPercent { get; set; }
        public double DefaultClosingCostOnBuy { get; set; }
        public double DefaultClosingCostOnSell { get; set; }
        public double BaseMiscExpenseMonthlyAmount { get; set; }

        public double BaseRepairMonthlyAmount { get; set; }
        public double BaseCapExPercentOfPropertyValue { get; set; }
        public double BaseHomeOwnerInsurancePercentageOfPropertyValue { get; set; }
        public double BasePropertyManagementCostAsPercentageOfMonthlyRent { get; set; }
    }
}
