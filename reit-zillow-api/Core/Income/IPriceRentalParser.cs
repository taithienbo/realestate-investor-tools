using Core.Dto;


namespace Core.Income
{
    public interface IPriceRentalParser
    {
        public PriceRentalDetail Parse(string html);
    }
}
