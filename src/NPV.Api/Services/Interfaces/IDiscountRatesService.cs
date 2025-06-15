namespace NPV.Api.Services.Interfaces
{
    public interface IDiscountRatesService
    {
        ValueTask<IEnumerable<decimal>> GetDiscountRates();
    }

}
