namespace Core.Zillow
{
    public interface IZillowClient
    {
        Task<string> GetListingHtmlPage(string address);
        Task<string> GetPriceMyRentalHtmlPage(string address);

        Task<string> SearchListingsByZipCode(int zipCode);
    }
}
