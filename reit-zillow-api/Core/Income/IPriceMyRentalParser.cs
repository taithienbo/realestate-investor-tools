using Core.Dto;


namespace Core.Income
{
    public interface IPriceMyRentalParser
    {
        public PriceMyRentalDetail Parse(string html);
    }
}
