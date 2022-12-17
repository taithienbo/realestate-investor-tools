using Core.Dto;

namespace Core.Listing
{
    public interface IZillowListingParser
    {
        ListingDetail Parse(string html);
        ListingDetail Get(string address);
    }
}
