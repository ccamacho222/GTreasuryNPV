using NPV.Api.Services.Implementations;

namespace NPV.Api.Tests
{
    [TestClass]
    public class DiscountRatesServiceTests
    {
        private DiscountRatesService _discountRatesService;

        [TestInitialize]
        public void Setup()
        {
            _discountRatesService = new DiscountRatesService();
        }

        [TestMethod]
        public async Task GetDiscountRates_ReturnsCorrectSequence()
        {
            // Arrange
            List<decimal> expectedRates = new List<decimal>();
            for (decimal i = 0.01m; i <= 0.15m; i += 0.0025m)
            {
                expectedRates.Add(i);
            }

            // Act
            IEnumerable<decimal> actualRates = await _discountRatesService.GetDiscountRates();

            // Assert
            Assert.AreEqual(expectedRates.Count, actualRates.Count(), "The number of generated discount rates is incorrect.");
            CollectionAssert.AreEqual(expectedRates, actualRates.ToList(), "The sequence of generated discount rates is incorrect.");
        }

        [TestMethod]
        public async Task GetDiscountRates_DoesNotReturnEmpty()
        {
            // Act
            IEnumerable<decimal> actualRates = await _discountRatesService.GetDiscountRates();

            // Assert
            Assert.IsNotNull(actualRates, "The returned collection of discount rates should not be null.");
            Assert.IsTrue(actualRates.Any(), "The returned collection of discount rates should not be empty.");
        }
    }
}
