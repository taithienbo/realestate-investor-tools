﻿using Core.ExpenseEstimator;


namespace Infrastructure.ExpenseEstimator
{
    public class CapExEstimator : ICapExExpenseEstimator
    {
        private const double BaseCapExPercentOfPropertyValue = .20;

        public double CalculateEstimatedMonthlyCapEx(double propertyValue, int propertyAge)
        {
            var monthlyBaseAmount = BaseCapExPercentOfPropertyValue / 100 * propertyValue / 12;
            double monthlyAdditionalAmountBasedOnAge = propertyAge;
            var estimatedMonthlyAmount = monthlyBaseAmount + monthlyAdditionalAmountBasedOnAge;
            return estimatedMonthlyAmount;
        }
    }
}