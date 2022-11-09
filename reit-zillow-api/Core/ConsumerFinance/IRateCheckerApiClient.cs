using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ConsumerFinance
{
    public interface IRateCheckerApiClient
    {
        Task<RateCheckerResponse?> CheckRate(RateCheckerRequestInfo request);
    }
}
