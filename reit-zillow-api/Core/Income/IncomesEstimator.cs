using Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Income
{
    public class IncomesEstimator : IIncomesEstimator
    {
        private readonly IPriceRentalService _priceRentalService;

        public IncomesEstimator(IPriceRentalService priceRentalService)
        {
            _priceRentalService = priceRentalService;
        }

        public async Task<IDictionary<string, double>> EstimateIncomes(string address)
        {
            double rentalIncome = await _priceRentalService.PriceMyRental(address);
            return new Dictionary<string, double>()
            {
                {nameof(CommonIncomeType.Rental), rentalIncome }
            };
        }
    }
}
