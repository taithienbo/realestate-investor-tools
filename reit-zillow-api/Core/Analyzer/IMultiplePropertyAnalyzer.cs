using Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Analyzer
{
    public interface IMultiplePropertyAnalyzer
    {
        public Task<IDictionary<string, PropertyAnalysisDetail>> AnalyzeProperties(int zipCode);
    }
}
