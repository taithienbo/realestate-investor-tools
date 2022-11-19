using Core.Constants;
using Moq;
using System.Buffers.Text;
using System;

namespace Core
{
    public class Calculators
    {
        public static double CalculateLoanAmount(double propertyPrice, double downPaymentPercentage)
        {
            var downPaymentAmount = (downPaymentPercentage / 100) * propertyPrice;
            return propertyPrice - downPaymentAmount;
        }

        public static double CalculateNetOperatingIncome(IDictionary<string, double> incomes, IDictionary<string, double> expenses)
        {
            // NOI = The profit you’ve received in a year, excluding your mortgage payment. 
            var totalMonthlyExpenseExcludingMortgage =
                expenses.Where(keyValuePair => keyValuePair.Key != Enum.GetName(CommonExpenseType.Mortgage)).Sum(keyValuePair => keyValuePair.Value);
            var totalMonthlyIncome = incomes.Sum(keyValuePair => keyValuePair.Value);
            return (totalMonthlyIncome * 12) - (totalMonthlyExpenseExcludingMortgage * 12);
        }

        /// <summary>
        /// Calculate the cap rate of an investment, given the property cash 
        /// price and monthly incomes. 
        /// Cap Rate: The percentage, or the return in investment you would 
        /// get if you pay all cash and have no mortgage. 
        /// "Cap rate is the rate of return an investor would buy the property 
        /// at, not considering the mortgage payment”.
        /// Ex: Let's say you purchase property at 4500000 (4.5 millions). 
        /// Let’s say we pay 4.5 millions in cash. And we make 450,000 profit 
        /// annually, then cap rate = 450,000/4500000 *100 = 10%

        /// </summary>
        /// <param name="propertyPrice">The cash price at which we purchase the property</param>
        /// <param name="monthlyIncomes">The incomes we receive monthly from the property</param>
        /// <returns></returns>
        public static double CalculateCapRate(double propertyPrice, IDictionary<string, double> monthlyIncomes)
        {
            double totalYearlyIncome = monthlyIncomes.Sum(keyValuePair => keyValuePair.Value) * 12;
            return totalYearlyIncome / propertyPrice * 100;
        }

        /// <summary>
        /// Calcualte the Debt Service Coverage Ratio (DBCR) of an investment. 
        /// 
        /// DBCR: Total debt payment includes mortgage and interest. 
        /// Banks want the DSCR to be at least 1.2. 
        /// Ex: Property brings $1 MIL in income, expenses total 400,000.
        /// Then, NOI = $600k. If total annual debt payment is $500k, then
        /// DSCR = NOI / Annual debt payment = 600k / 500k = 1.2 

        /// </summary>
        /// <param name="monthlyIncomes"></param>
        /// <param name="monthlyExpenses"></param>
        /// <returns></returns>
        public static double CalculateDebtServiceCoverageRatio(IDictionary<string, double> monthlyIncomes, IDictionary<string, double> monthlyExpenses)
        {
            double annualDebtPayment = monthlyExpenses.Where(keyValuePair => keyValuePair.Key == nameof(CommonExpenseType.Mortgage)).FirstOrDefault().Value * 12;

            return CalculateNetOperatingIncome(monthlyIncomes, monthlyExpenses) / annualDebtPayment;
        }

        /// <summary>
        /// Calculate the monthly amount of cash remaining after all expenses.
        /// Income is the monthly income you receive every month. Typically, 
        /// this will be rental income. But it can be also other incomes.For 
        /// example, you may have laundry machines that operates by coin and 
        /// receive income from it.
        /// Expense includes cost of repair, maintenance, management fee, 
        /// insurance, mortage etc...
        /// Cash flow = Incomes - Expenses
        /// </summary>
        /// <param name="incomes"></param>
        /// <param name="expenses"></param>
        /// <returns></returns>
        public static double CalculateCashFlow(IDictionary<string, double> incomes,
            IDictionary<string, double> expenses)
        {
            return incomes.Sum(keyValue => keyValue.Value) - expenses.Sum(keyValue => keyValue.Value);
        }

        /// <summary>
        /// The cash-on-cash return on investment (often abbreviated as CoCROI)
        /// is a simple metric that tells us what kind of yield our money is
        /// making us based only on the cash flow(ignoring appreciation, tax
        /// benefits, and the loan pay down). The CoCROI is nice because it 
        /// allows us to compare this investment against other investments, 
        /// like the stock market or mutual funds."
        /// "CoCROI is simply the ratio between how much cash flow we received 
        /// over a one-year period and how much money we invested."
        /// Annual Cash Flow = Total income - Total expenses
        /// Total investment is the total money you put out of pocket to 
        /// purchase the deal. 

        /// </summary>
        /// <param name="monthlyIncomes"></param>
        /// <param name="monthlyExpenses"></param>
        /// <param name="totalInvestment"></param>
        /// <returns></returns>
        public static double CalculateCashOnCashReturn(
            IDictionary<string, double> monthlyIncomes, IDictionary<string, double> monthlyExpenses, double totalInvestment)
        {
            return CalculateCashFlow(monthlyIncomes, monthlyExpenses) * 12 / totalInvestment * 100;
        }

        public static double CalculateDownPayment(double listingPrice, double downPaymentPercent)
        {
            return downPaymentPercent / 100 * listingPrice;
        }
    }
}
