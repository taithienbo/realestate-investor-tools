namespace Core.ConsumerFinance
{
    public record RateCheckerRequestInfo
    {
        public double LoanAmount { get; set; }
        public double Price { get; set; }
        public string State { get; set; } = "ca";
        public string LoanType { get; set; } = "conf";
        public int MinFico { get; set; } = 700;
        public int MaxFico { get; set; } = 760;
        public string RateStructure { get; set; } = "Fixed";
        public int LoanTerm { get; set; } = 30;


    }
}
