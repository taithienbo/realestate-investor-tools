using Core.Constants;

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
    }
}
