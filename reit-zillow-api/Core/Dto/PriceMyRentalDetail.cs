

namespace Core.Dto
{
    public class PriceRentalDetail
    {
        public double ZEstimateLow { get; set; }
        public double ZEstimateHigh { get; set; }
        public double ZEstimate { get; set; }
        public string PropertyAddress { get; set; } = null!;
    }
}
