using Core.CashFlow;
using Core.Constants;
using Core.Expense;
using Core.Income;
using Core.Listing;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Tests.CashFlow
{
    public class CashFlowCalculatorTests
    {
        private readonly ICashFlowService _cashFlowCalculator;
        private readonly Mock<IIncomesService> _mockIncomeEstimator;
        private readonly Mock<IExpenseService> _mockExpenseService;

        public CashFlowCalculatorTests()
        {
            _mockIncomeEstimator = new Mock<IIncomesService>();
            _mockExpenseService = new Mock<IExpenseService>();

            _cashFlowCalculator = new CashFlowService(_mockIncomeEstimator.Object, _mockExpenseService.Object);
        }

        [Fact]
        public async void CalculateCashFlowByAddress()
        {
            // arrange 
            const string Address = "123 Test St";

            const double ExpectedRentalIncome = 2700;
            var expectedIncomes = new Dictionary<string, double>()
            {
                {nameof(CommonIncomeType.Rental), ExpectedRentalIncome }
            };
            _mockIncomeEstimator.Setup(incomeEstimator => incomeEstimator.EstimateIncomes(Address)).ReturnsAsync(expectedIncomes);

            const double ExpectedMortgageExpense = 2400;
            var expectedExpenses = new Dictionary<string, double>()
            {
                {nameof(CommonExpenseType.Mortgage), ExpectedMortgageExpense }
            };
            _mockExpenseService.Setup(expenseService => expenseService.CalculateExpenses(Address)).ReturnsAsync(expectedExpenses);

            double expectedCashFlow = ExpectedRentalIncome - ExpectedMortgageExpense;

            // act 
            double cashFlow = await _cashFlowCalculator.CalculateCashFlow(Address);
            // assert 
            Assert.Equal(expectedCashFlow, cashFlow, 0);
        }
    }
}
