using Core.Dto;
using Core.Expense;
using Moq;
using reit_zillow_api.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Tests
{
    public class ExpenseControllerTests
    {
        private ExpenseController _expenseController;
        private Mock<IExpenseEstimator> _mockExpenseEstimator;

        public ExpenseControllerTests()
        {
            _mockExpenseEstimator = new Mock<IExpenseEstimator>();

            _expenseController = new ExpenseController(_mockExpenseEstimator.Object);
        }

        [Fact]
        public void EstimateExpenses()
        {
            var estimateExpenseRequest = new EstimateExpensesRequest();
            var expectedExpenseDetail = new ExpenseDetail();
            _mockExpenseEstimator.Setup(estimator => estimator.EstimateExpenses(estimateExpenseRequest)).Returns(expectedExpenseDetail);
            var expenseDetail = _expenseController.EstimateExpenses(estimateExpenseRequest);
            Assert.NotNull(expenseDetail);
        }

        [Fact]
        public void EstimateExpenses_ThrowExceptionOnNullArgument()
        {
            // arrange 
            EstimateExpensesRequest? estimateExpenseRequest = null;
            // assert
#pragma warning disable CS8604 // Possible null reference argument.
            var exception = Assert.Throws<ArgumentNullException>(() => _expenseController.EstimateExpenses(estimateExpenseRequest));
#pragma warning restore CS8604 // Possible null reference argument.
        }
    }
}
