using Core.Dto;


namespace Core.Analyzer
{
    public interface IPropertyAnalyzer
    {
        public Task<PropertyAnalysisDetail?> AnalyzeProperty(string address);


    }
}
