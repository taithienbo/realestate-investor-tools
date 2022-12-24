using Core.Dto;
using Core.Income;
using Core.Interest;
using Core.Listing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Expense
{
    public class ExpenseService : IExpenseService
    {
        private IExpenseEstimator _expenseEstimator;
        private IMortgageInterestEstimator _mortgageInterestEstimator;
        private IPriceRentalService _priceRentalService;
        private IListingService _listingService;

        public ExpenseService(IListingService listingService,
            IPriceRentalService priceRentalService,
            IMortgageInterestEstimator mortgageInterestEstimator,
            IExpenseEstimator expenseEstimator)
        {
            _listingService = listingService;
            _priceRentalService = priceRentalService;
            _mortgageInterestEstimator = mortgageInterestEstimator;
            _expenseEstimator = expenseEstimator;
        }

        public async Task<IDictionary<string, double>> CalculateExpenses(string address)
        {
            var listingDetail = await _listingService.GetListingDetail(address);
            var interestRate = await _mortgageInterestEstimator.GetCurrentInterest(listingDetail.ListingPrice);
            var rentAmount = await _priceRentalService.PriceMyRental(address);

            return _expenseEstimator.EstimateExpenses(new EstimateExpensesRequest()
            {
                PropertyAge = listingDetail.PropertyAge,
                PropertyValue = listingDetail.ListingPrice,
                InterestRate = interestRate,
                RentAmount = rentAmount,
                HoaFee = listingDetail.HoaFee
            });
        }
    }
}
