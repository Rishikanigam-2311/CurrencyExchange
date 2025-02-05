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
    [HttpGet("convert")]
    public async Task<IActionResult> ConvertCurrency([FromQuery] string sourceCurrency, [FromQuery] string targetCurrency, [FromQuery] double amount)
    //public async Task<IActionResult> ConvertCurrency([FromQuery] string sourceCurrency, [FromQuery] string targetCurrency, [FromQuery] double amount, string date)
    {
        try
        {
            var convertedAmount = await _conversionService.ConvertCurrencyAsync(sourceCurrency, targetCurrency, amount);
            //var convertedAmount = await _conversionService.ConvertCurrencyAsync(sourceCurrency, targetCurrency, amount, date);
            return Ok(new { Amount = amount, ConvertedAmount = convertedAmount });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Endpoint for real-time conversion of specifica date
    //[HttpGet("convertfordate")]
    //public async Task<IActionResult> ConvertCurrencyForDate([FromQuery] string sourceCurrency, [FromQuery] string targetCurrency, [FromQuery] double amount, string date)
    //{
    //    try
    //    {
    //        var convertedAmount = await _conversionService.ConvertCurrencyAsyncForDate(sourceCurrency, targetCurrency, amount, date);
    //        return Ok(new { Amount = amount, ConvertedAmount = convertedAmount });
    //    }
    //    catch (Exception ex)
    //    {
    //        return BadRequest(ex.Message);
    //    }
    //}
}




