using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Core.Loan
{
    public enum LoanProgram
    {
        ThirtyYearFixed
    }

    public static class LoanProgramExtensions
    {
        public static int NumberOfMonths(this LoanProgram loanProgram)
        {
            switch (loanProgram)
            {
                case LoanProgram.ThirtyYearFixed:
                    return 360;
                default:
                    return 0;
            }
        }

    }
}
