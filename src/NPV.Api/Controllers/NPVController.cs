using Microsoft.AspNetCore.Mvc;
using NPV.Api.Models;
using NPV.Api.Services.Interfaces;

namespace NPV.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NPVController : ControllerBase
    {
        private readonly IDiscountRatesService _discountRatesService;
        private readonly INPVCalculatorService _npvCalculatorService;
        private readonly ILogger<NPVController> _logger;

        public NPVController(
            INPVCalculatorService npvCalculatorService,
            IDiscountRatesService discountRatesService, 
            ILogger<NPVController> logger)
        {
            _npvCalculatorService = npvCalculatorService;
            _discountRatesService = discountRatesService;
            _logger = logger;
        }

        [HttpGet("discounts")]
        public async Task<ActionResult<IEnumerable<decimal>>> GetAvailableDiscountRates()
        {
            _logger.LogInformation("{0} called", nameof(GetAvailableDiscountRates));
            IEnumerable<decimal> rates;

            try
            {
                rates = await _discountRatesService.GetDiscountRates();
                return Ok(rates);
            }
            catch (Exception ex)
            {
                //log the exception and return a 500
                _logger.LogError(ex, "{0}", nameof(GetAvailableDiscountRates));
                return StatusCode(500, new ProblemDetails
                {
                    Status = 500,
                    Title = "An unexpected error occurred.",
                    Detail = "An unexpected error occurred.",
                    Instance = HttpContext.Request.Path
                });
            }
        }

        [HttpPost("")]
        public async Task<ActionResult<decimal>> CalculateNetPresentValue(NPVRequest request)
        {
            _logger.LogInformation("{0} called", nameof(CalculateNetPresentValue));
            decimal netPresentValue;

            try
            {
                netPresentValue = await _npvCalculatorService.CalculateNetPresentValue(request.DiscountRate, request.Cashflow);
                return Ok(netPresentValue);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                //log the exception and return a 500
                _logger.LogError(ex, "{0}", nameof(CalculateNetPresentValue));
                return BadRequest();
            }
            catch (Exception ex)
            {
                //log the exception and return a 500
                _logger.LogError(ex, "{0}", nameof(CalculateNetPresentValue));
                return StatusCode(500, new ProblemDetails
                {
                    Status = 500,
                    Title = "An unexpected error occurred.",
                    Detail = "An unexpected error occurred.",
                    Instance = HttpContext.Request.Path
                });
            }
        }
    }
}
