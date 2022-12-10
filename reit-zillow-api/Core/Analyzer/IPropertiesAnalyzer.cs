using Core.Dto;


namespace Core.Analyzer
{
    public interface IPropertiesAnalyzer
    {
        public Task<IDictionary<string, PropertyAnalysisDetail>> AnalyzeProperties(int zipCode);

        public Task<PropertyAnalysisDetail?> Analyze(string address);
    }
}
