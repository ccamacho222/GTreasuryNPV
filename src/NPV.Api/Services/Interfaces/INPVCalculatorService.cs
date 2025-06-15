namespace NPV.Api.Services.Interfaces
{
    public interface INPVCalculatorService
    {
        ValueTask<decimal> CalculateNetPresentValue(decimal discountRate, decimal[] cashflow);
    }
}
