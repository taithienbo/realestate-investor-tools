using Core.Dto;

namespace Core.Listing
{
    public interface IListingParser
    {
        ListingDetail Parse(string html);
    }
}
