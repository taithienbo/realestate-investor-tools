namespace Core.ConsumerFinance
{
    public interface IRateCheckerApiClient
    {
        Task<RateCheckerResponse?> CheckRate(RateCheckerRequestInfo request);
    }
}
