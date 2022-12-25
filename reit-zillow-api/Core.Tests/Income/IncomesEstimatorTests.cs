using Core.Constants;
using Core.Income;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Tests.Income
{
    public class IncomesEstimatorTests
    {
        private readonly IIncomesService _incomesEstimator;
        private readonly Mock<IPriceRentalService> _priceRentalService;

        public IncomesEstimatorTests()
        {
            _priceRentalService = new Mock<IPriceRentalService>();
            _incomesEstimator = new IncomesEstimator(_priceRentalService.Object);
        }

        [Fact]
        public void EstimateIncomesGivenAddress()
        {
            // arrange 
            const string Address = "123 Test Address";

            const double ExpectedRentalIncome = 3000;
            _priceRentalService.Setup(priceRentalService => priceRentalService.PriceMyRental(Address)).ReturnsAsync(ExpectedRentalIncome);

            var expectedIncomes = new Dictionary<string, double>()
            {
                { nameof(CommonIncomeType.Rental), ExpectedRentalIncome}
            };
            // act 
            IDictionary<string, double> actualIncomes = _incomesEstimator.EstimateIncomes(Address).Result;
            // assert
            Assert.NotNull(actualIncomes);
            Assert.Equal(expectedIncomes.Count, actualIncomes.Count);
            Assert.True(actualIncomes.Keys.All(key => expectedIncomes.ContainsKey(key)));
            Assert.True(actualIncomes.Keys.All(key => expectedIncomes[key] == actualIncomes[key]));
        }
    }
}
