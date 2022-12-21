using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Income
{
    public interface IPriceRentalService
    {
        public Task<double> PriceMyRental(string address);
    }
}
