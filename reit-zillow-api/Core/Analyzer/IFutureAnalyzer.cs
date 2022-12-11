using Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Analyzer
{
    public interface IFutureAnalyzer
    {
        public double CalculateNetProfitsOnSell(InvestmentOnSellAnalyzerParams parameters);
    }
}
