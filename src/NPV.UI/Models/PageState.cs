namespace NPV.UI.Models
{
    public class PageState
    {
        public List<decimal> DiscountRates { get; set; }
        public decimal CurrentDiscountRate { get; set; }

        //represents series of income/loss per time period
        public List<decimal> Cashflow { get; set; }

        public decimal? NetPresentValue { get; set; }
        public string NPVErrorMessage { get; set; } = string.Empty;

    }
}
