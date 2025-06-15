namespace NPV.UI.Services
{
    public interface IApiService
    {
        ValueTask<List<decimal>> GetAvailableDiscountRates();

        ValueTask<decimal> CalculateNetPresentValue(decimal discountRate, List<decimal> cashflows);
    }
}
