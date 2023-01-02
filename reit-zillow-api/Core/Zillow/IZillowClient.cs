namespace Core.Zillow
{
    public interface IZillowClient
    {

        Task<string> SearchListingsByZipCode(int zipCode);
    }
}
