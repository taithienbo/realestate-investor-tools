namespace Core.ConsumerFinance
{
    public record RateCheckerResponse
    {
        public IDictionary<double, int> Data { get; set; } = null!;
        public RateCheckerRequestInfo Request { get; set; } = null!;
    }
}
