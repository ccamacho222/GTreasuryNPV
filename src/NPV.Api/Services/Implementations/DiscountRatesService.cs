using NPV.Api.Services.Interfaces;

namespace NPV.Api.Services.Implementations
{
    public class DiscountRatesService : IDiscountRatesService
    {
        public async ValueTask<IEnumerable<decimal>> GetDiscountRates()
        {
            List<decimal> discountRates = new();
            for (decimal i = 0.01m; i <= 0.15m; i += 0.0025m)
            {
                discountRates.Add(i);
            }
            return discountRates;
        }
    }
}
