using Core.Constants;
using Core.Dto;
using Core.Expense;
using Core.Income;
using Core.Interest;
using Core.Listing;
using Moq;
using System.Net.Http.Headers;

namespace Core.Tests.Expense
{
    public class ExpenseServiceTests
    {
        private readonly IExpenseService _expenseService;
        private readonly Mock<IListingService> _mockListingService;
        private readonly Mock<IPriceRentalService> _mockPriceRentalService;
        private readonly Mock<IMortgageInterestEstimator> _mockMortgageInterestEstimator;
        private readonly Mock<IExpenseEstimator> _mockExpenseEstimator;

        public ExpenseServiceTests()
        {
            _mockListingService = new Mock<IListingService>();
            _mockPriceRentalService = new Mock<IPriceRentalService>();
            _mockMortgageInterestEstimator = new Mock<IMortgageInterestEstimator>();
            _mockExpenseEstimator = new Mock<IExpenseEstimator>();

            _expenseService = new ExpenseService(_mockListingService.Object, _mockPriceRentalService.Object, _mockMortgageInterestEstimator.Object,
                _mockExpenseEstimator.Object);
        }

        [Fact]
        public async void EstimateExpensesByAddress()
        {
            Random random = new Random();

            // arrange 
            const string Address = "123 Test Ave";

            _mockListingService.Setup(service => service.GetListingDetail(Address)).ReturnsAsync(new ListingDetail());
            _mockMortgageInterestEstimator.Setup(interestEstimator => interestEstimator.GetCurrentInterest(It.IsAny<double>())).ReturnsAsync(random.NextDouble());
            _mockPriceRentalService.Setup(service => service.PriceMyRental(Address)).ReturnsAsync(random.NextDouble());
            IDictionary<string, double> expectedExpenses = new Dictionary<string, double>()
            {
                { nameof(CommonExpenseType.Mortgage), 3000 }
            };
            _mockExpenseEstimator.Setup(expenseEstimator => expenseEstimator.EstimateExpenses(It.IsAny<EstimateExpensesRequest>())).Returns(expectedExpenses);
            // act 
            IDictionary<string, double> actualExpenses = await _expenseService.CalculateExpenses(Address);
            Assert.NotNull(actualExpenses);
        }
    }
}
