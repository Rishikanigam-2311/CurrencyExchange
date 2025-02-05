using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class CurrencyController : ControllerBase
{
    private readonly CurrencyConversionService _conversionService;

    // Inject the CurrencyConversionService into the controller
    public CurrencyController(CurrencyConversionService conversionService)
    {
        _conversionService = conversionService;
    }

    // Endpoint for real-time conversion
    [HttpGet("calculator")]
    //public async Task<IActionResult> ConvertCurrency([FromQuery] string first, [FromQuery] string target, [FromQuery] double amount)
    public async Task<IActionResult> ConvertCurrency( string Source_Currency,  string Target_Currency, double Amount, string? Date = null)
    {
        try
        {
            //var convertedAmount = await _conversionService.ConvertCurrencyAsync(first, target, amount);
            var convertedAmount = await _conversionService.ConvertCurrencyAsync(Source_Currency, Target_Currency, Amount, Date);
            return Ok(new { amount = Amount, ConvertedAmount = convertedAmount });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Endpoint for real-time conversion of specifica date
    //[HttpGet("convertfordate")]
    //public async Task<IActionResult> ConvertCurrencyForDate([FromQuery] string first, [FromQuery] string target, [FromQuery] double amount, string date)
    //{
    //    try
    //    {
    //        var convertedAmount = await _conversionService.ConvertCurrencyAsyncForDate(first, target, amount, date);
    //        return Ok(new { Amount = amount, ConvertedAmount = convertedAmount });
    //    }
    //    catch (Exception ex)
    //    {
    //        return BadRequest(ex.Message);
    //    }
    //}
}




