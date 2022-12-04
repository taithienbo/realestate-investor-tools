using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto
{
    public record AmortizationScheduleEntry
    {
        public double Principal { get; set; }
        public double Interest { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public double RemainingBalance { get; set; }
    }
}
