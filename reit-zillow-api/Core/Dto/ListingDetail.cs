namespace Core.Dto
{
    public class ListingDetail
    {
        public double ListingPrice { get; set; }
        public int NumOfBedrooms { get; set; }
        public int NumOfBathrooms { get; set; }
        public int NumOfStories { get; set; }
        public string? NumOfLevels { get; set; }
        public int NumOfParkingSpaces { get; set; }
        public string? LotSize { get; set; }
        public int NumOfGarageSpaces { get; set; }
        public string? HomeType { get; set; }
        public string? PropertyCondition { get; set; }
        public int YearBuilt { get; set; }
        public bool HasHOA { get; set; }
        public int PropertyAge
        {
            get
            {
                return DateTime.Now.Year - this.YearBuilt;
            }
        }
    }
}
