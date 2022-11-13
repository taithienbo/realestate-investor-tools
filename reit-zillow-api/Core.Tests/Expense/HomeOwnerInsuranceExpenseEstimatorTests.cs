using Core.Expense;

namespace Infrastructure.Tests.Expense
{

    public class HomeOwnerInsuranceExpenseEstimatorTests
    {
        private IHomeOwnerInsuranceExpenseEstimator _homeOwnerInsuranceCalculator;

        public HomeOwnerInsuranceExpenseEstimatorTests()
        {
            _homeOwnerInsuranceCalculator = new HomeOwnerInsuranceExpenseEstimator();
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
