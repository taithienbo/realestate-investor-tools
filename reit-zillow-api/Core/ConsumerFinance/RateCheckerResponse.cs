using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ConsumerFinance
{
    public record RateCheckerResponse
    {
        public IDictionary<double, int> Data { get; set; } = null!;
        public RateCheckerRequestInfo Request { get; set; } = null!;
    }
}
