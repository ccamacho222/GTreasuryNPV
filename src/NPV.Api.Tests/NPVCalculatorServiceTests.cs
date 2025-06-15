using NPV.Api.Services.Implementations;

namespace NPV.Api.Tests
{
    [TestClass]
    public class NPVCalculatorServiceTests
    {
        private NPVCalculatorService _npvCalculatorService;

        [TestInitialize]
        public void Setup()
        {
            _npvCalculatorService = new NPVCalculatorService();
        }

        [TestMethod]
        public async Task CalculateNPV_MultiYearCashflow_ShouldReturnCorrectValue()
        {
            // Arrange
            decimal discountRate = 0.10m; // 10%
            decimal[] cashFlows = { -100000.00m, 30000.00m, 40000.00m, 50000.00m, 30000.00m };
            decimal expectedNpv = 18386.72221m; // Adjust based on your exact calculation precision

            // Act
            decimal actualNpv = await _npvCalculatorService.CalculateNetPresentValue(discountRate, cashFlows);

            // Assert
            decimal delta = 0.0001m; // Allow for very small differences
            Assert.AreEqual(expectedNpv, actualNpv, delta, "NPV calculation for standard scenario is incorrect.");
        }

        [TestMethod]
        public async Task CalculateNPV_MultiYearCashflow2_ShouldReturnCorrectValue()
        {
            // Arrange
            decimal discountRate = 0.05m; // 5%
            decimal[] cashFlows = { -5000.00m, 2000.00m, 2500.00m, 3000.00m };
            decimal expectedNpv = 1763.84839m; // Rounded to 5 decimal places

            // Act
            decimal actualNpv = await _npvCalculatorService.CalculateNetPresentValue(discountRate, cashFlows);

            // Assert
            decimal delta = 0.00001m;
            Assert.AreEqual(expectedNpv, actualNpv, delta, "NPV calculation for another scenario is incorrect.");
        }

        [TestMethod]
        public async Task CalculateNPV_OneInvestment_ShouldReturnUndiscountedValues()
        {
            // Arrange
            decimal discountRate = 0.00m; // 0%
            decimal[] cashFlows = { -1000.00m, 500.00m, 700.00m };
            decimal expectedNpv = -1000.00m + 500.00m + 700.00m; // Should be sum of cash flows

            // Act
            decimal actualNpv = await _npvCalculatorService.CalculateNetPresentValue(discountRate, cashFlows);

            // Assert
            Assert.AreEqual(expectedNpv, actualNpv, "NPV calculation with zero discount rate is incorrect.");
        }

        [TestMethod]
        public async Task CalculateNPV_OneInvestment_ShouldReturnCorrectValue()
        {
            // Arrange
            decimal discountRate = 0.10m;
            decimal[] cashFlows = { -50000.00m };
            decimal expectedNpv = -50000.00m;

            // Act
            decimal actualNpv = await _npvCalculatorService.CalculateNetPresentValue(discountRate, cashFlows);

            // Assert
            Assert.AreEqual(expectedNpv, actualNpv, "NPV calculation with only initial investment is incorrect.");
        }

        // --- Edge Case / Boundary Test Cases ---

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public async Task CalculateNPV_NegativeDiscountRate_ThrowException()
        {
            // Arrange
            decimal discountRate = -0.05m; // Negative discount rate
            decimal[] cashFlows = { -100m, 50m, 60m };

            // Act & Assert
            await _npvCalculatorService.CalculateNetPresentValue(discountRate, cashFlows);
        }

        [TestMethod]
        public async Task CalculateNPV_NullCashFlows_ReturnZero()
        {
            // Arrange
            decimal discountRate = 0.10m;
            decimal[] cashFlows = null; // Null cash flow array
            decimal expectedNpv = 0m;

            // Act
            decimal actualNpv = await _npvCalculatorService.CalculateNetPresentValue(discountRate, cashFlows);

            // Assert
            Assert.AreEqual(expectedNpv, actualNpv, "NPV calculation with null cash flows should return 0.");
        }

        [TestMethod]
        [Description("Ensures that an empty cash flow array returns 0.")]
        public async Task CalculateNPV_EmptyCashFlow_ReturnZero()
        {
            // Arrange
            decimal discountRate = 0.10m;
            decimal[] cashFlows = new decimal[0]; // Empty cash flow array
            decimal expectedNpv = 0m;

            // Act
            decimal actualNpv = await _npvCalculatorService.CalculateNetPresentValue(discountRate, cashFlows);

            // Assert
            Assert.AreEqual(expectedNpv, actualNpv, "NPV calculation with empty cash flows should return 0.");
        }
    }
}
