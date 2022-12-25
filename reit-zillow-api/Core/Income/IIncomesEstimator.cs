using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Income
{
    public interface IIncomesService
    {
        public Task<IDictionary<string, double>> EstimateIncomes(string address);
    }
}
