using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class LoanCalculatorUtil
    {
        public static double CalculateLoanAmount(double propertyPrice, double downPaymentPercentage)
        {
            var downPaymentAmount = (downPaymentPercentage / 100) * propertyPrice;
            return propertyPrice - downPaymentAmount;
        }
    }
}
