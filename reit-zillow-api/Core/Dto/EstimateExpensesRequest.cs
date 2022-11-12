using Core.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto
{
    public record EstimateExpensesRequest
    {
        [Range(1, double.MaxValue)]
        public double PropertyValue { get; set; }
        [Range(1, int.MaxValue)]
        public int PropertyAge { get; set; }
        [Range(1, 100)]
        public double DownPaymentPercent { get; set; } = 25;
        [Range(1, double.MaxValue)]
        public double InterestRate { get; set; }
        public string LoanProgram { get; set; } = "ThirtyYearFixed";
        [Range(1, double.MaxValue)]
        public double RentAmount { get; set; }


    }
}
