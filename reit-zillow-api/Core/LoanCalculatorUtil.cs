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
    }
}
