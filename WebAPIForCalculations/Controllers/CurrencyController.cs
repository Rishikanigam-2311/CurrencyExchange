using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class CurrencyController : ControllerBase
{
    private readonly CurrencyConversionService _conversionService;

    public CurrencyController(CurrencyConversionService conversionService)
    {
        _conversionService = conversionService;
    }
    [HttpGet("calculator")]
    public async Task<IActionResult> ConvertCurrency(string Source_Currency, string Target_Currency, double Amount, string? Date = null)
    {
        try
        {
            var convertedAmount = await _conversionService.ConvertCurrencyAsync(Source_Currency, Target_Currency, Amount, Date);
            return Ok(new { amount = Amount, ConvertedAmount = convertedAmount });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}




