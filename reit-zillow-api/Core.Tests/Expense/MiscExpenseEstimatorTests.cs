using Core.Expense;
using Core.Options;

namespace Infrastructure.Tests.Expense
{
    public class MiscExpenseEstimatorTests
    {
        private IMiscExpenseEstimator _estimator;
        private readonly AppOptions _appOptions;

        public MiscExpenseEstimatorTests()
        {
            _appOptions = new AppOptions()
            {
                BaseMiscExpenseMonthlyAmount = 100
            };

            _estimator = new MiscExpenseEstimator(_appOptions);
        }

        [Fact]
        public void EstimateMonthlyAmount()
        {
            // arrange
            var expectedMiscMonthlyAmount = _appOptions.BaseMiscExpenseMonthlyAmount;
            // act 
            var actualMiscMonthlyAmount = _estimator.EstimateMonthlyAmount();
            // assert
            Assert.Equal(expectedMiscMonthlyAmount, actualMiscMonthlyAmount, 0);
        }
    }
}
