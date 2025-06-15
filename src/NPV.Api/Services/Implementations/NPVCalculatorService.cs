using NPV.Api.Services.Interfaces;

namespace NPV.Api.Services.Implementations
{
    public class NPVCalculatorService : INPVCalculatorService
    {
        public async ValueTask<decimal> CalculateNetPresentValue(decimal discountRate, decimal[] cashflow)
        {
            //ensure discount rate is positive (there are rare cases where rate is negative, but we'll not cover it here for now)
            if (discountRate < 0) { throw new ArgumentOutOfRangeException(nameof(discountRate)); }
            //if cashflow is empty
            if (cashflow is null || cashflow.Length < 1) { return 0m; }
            
            decimal npv = 0;
            decimal discountFactor = 1;
            decimal discountMultiplier = 1.0m /  (1.0m + discountRate);

            for (int timePeriod = 0; timePeriod < cashflow.Length; timePeriod++)
            {
                npv += cashflow[timePeriod] * discountFactor;
                discountFactor *= discountMultiplier;
            }
            return npv;
        }
    }
}
