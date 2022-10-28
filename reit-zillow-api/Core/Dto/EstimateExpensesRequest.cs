using Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto
{
    public record EstimateExpensesRequest
    {
        public double PropertyValue { get; set; }
        public int PropertyAge { get; set; }
        public double DownPaymentPercent { get; set; }
        public double InterestRate { get; set; }
        public LoanProgram LoanProgram { get; set; }
        public double RentAmount { get; set; }


    }
}
