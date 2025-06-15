using System.Text.Json.Serialization;

namespace NPV.Api.Models
{
    public class NPVRequest
    {
        [JsonPropertyName("discountRate")]
        public decimal DiscountRate { get; set; }

        [JsonPropertyName("cashflow")]
        public decimal[] Cashflow { get; set; } = new decimal[0];
    }
}
