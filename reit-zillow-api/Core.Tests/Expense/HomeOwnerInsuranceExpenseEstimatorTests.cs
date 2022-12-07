using Core.Expense;
using Core.Options;

namespace Infrastructure.Tests.Expense
{

    public class HomeOwnerInsuranceExpenseEstimatorTests
    {
        private IHomeOwnerInsuranceExpenseEstimator _homeOwnerInsuranceCalculator;
        private readonly AppOptions _appOptions;

        public HomeOwnerInsuranceExpenseEstimatorTests()
        {
            _appOptions = new AppOptions()
            {
                BaseHomeOwnerInsurancePercentageOfPropertyValue = 0.25
            };
            _homeOwnerInsuranceCalculator = new HomeOwnerInsuranceExpenseEstimator(_appOptions);
        }

        [Fact]
        public void CalculateEstimatedMonthlyAmount()
        {
            // arrange 
            var listingPrice = 600000;
            var expectedMonthlyAmount = 125; // listingPrice * .25 / 100 / 12;
            var actualMonthlyAmount = _homeOwnerInsuranceCalculator
                .CalculateMonthlyAmount(listingPrice);
            Assert.Equal(expectedMonthlyAmount, actualMonthlyAmount, 0);
        }
    }
}
