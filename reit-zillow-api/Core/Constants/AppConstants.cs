using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Constants
{
    public static class PropertyCondition
    {
        public const string TurnKey = nameof(TurnKey);
        public const string Fixer = nameof(Fixer);
    }

    public static class HomeType
    {
        public const string SingleFamily = nameof(SingleFamily);
    }

    public enum LoanProgram
    {
        ThirtyYearFixed
    }
}
