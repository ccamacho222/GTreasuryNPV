using Moq;
using Moq.Protected;
using NPV.UI.Services;
using System.Net;
using System.Net.Http.Json;

namespace NPV.UI.Tests
{
    [TestClass]
    public class ApiServiceTests
    {
        private Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private HttpClient _httpClient;
        private ApiService _apiService;

        [TestInitialize]
        public void Setup()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("http://test.com/") // Set a base address for HttpClient
            };
            _apiService = new ApiService(_httpClient);
        }

        [TestMethod]
        public async Task CalculateNetPresentValue_Success_ReturnsNpv()
        {
            // Arrange
            decimal expectedNpv = 123.45m;
            decimal discountRate = 0.05m;
            List<decimal> cashflow = new List<decimal> { 100m, 200m, 300m };

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(expectedNpv)
                })
                .Verifiable();

            // Act
            decimal actualNpv = await _apiService.CalculateNetPresentValue(discountRate, cashflow);

            // Assert
            Assert.AreEqual(expectedNpv, actualNpv);

            _mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri == new Uri("http://test.com/npv") &&
                    req.Content.ReadAsStringAsync().Result.Contains("\"discountRate\":0.05") &&
                    req.Content.ReadAsStringAsync().Result.Contains("\"cashflow\":[100,200,300]")
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Failed to calculate NPV: InternalServerError - Test Error")]
        public async Task CalculateNetPresentValue_Failure_ThrowsException()
        {
            // Arrange
            decimal discountRate = 0.05m;
            List<decimal> cashflow = new List<decimal> { 100m, 200m, 300m };
            string errorMessage = "Test Error";

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent(errorMessage) // Use StringContent for error message
                })
                .Verifiable();

            // Act
            await _apiService.CalculateNetPresentValue(discountRate, cashflow);

            // Assert (ExpectedException handles the assertion)
        }

        [TestMethod]
        public async Task GetAvailableDiscountRates_Success_ReturnsList()
        {
            // Arrange
            List<decimal> expectedRates = new List<decimal> { 0.01m, 0.02m, 0.03m };

            // Mock SendAsync to return a successful response with a list of decimals
            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(expectedRates)
                })
                .Verifiable();

            // Act
            List<decimal> actualRates = await _apiService.GetAvailableDiscountRates();

            // Assert
            CollectionAssert.AreEquivalent(expectedRates, actualRates);

            _mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri == new Uri("http://test.com/npv/discounts")
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Null retrun value from GetAvailableDiscountRates() should throw an exception")]
        public async Task GetAvailableDiscountRates_NullContent_ReturnsEmptyList()
        {
            // Arrange
            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("null") // Simulate null JSON response for collection
                })
                .Verifiable();

            // Act
            List<decimal> actualRates = await _apiService.GetAvailableDiscountRates();

            // Assert
            
        }

    }
}
