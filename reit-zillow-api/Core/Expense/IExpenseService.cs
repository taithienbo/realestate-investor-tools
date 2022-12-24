using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Expense
{
    public interface IExpenseService
    {
        public Task<IDictionary<string, double>> CalculateExpenses(string address);
    }
}
