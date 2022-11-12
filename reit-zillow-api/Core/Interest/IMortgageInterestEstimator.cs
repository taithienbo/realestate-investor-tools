using Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interest
{
    public interface IMortgageInterestEstimator
    {
        public Task<double> GetCurrentInterest(double loanAmount, double propertyPrice);

        public Task<double> GetCurrentInterest(double propertyPrice);
    }
}
