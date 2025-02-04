using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ExchangeCurrencyAPIService;

[ApiController]
[Route("[Controllers]")]
public class CurrencyExchnageAPISercvice : ControllerBase
{
    public async Task<Dictionary<string, double>> GetCurrencyExchangeRates(string date)
    
    {
        string url = $"{AppConfiguration.Base_Url}{date}?access_key={AppConfiguration.Api_Key}";
        Console.WriteLine(url);

        using HttpClient client = new();

        HttpResponseMessage response = await client.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Unable to fetch exhange rate: {response.StatusCode}");
        }
        
        string responseData = await response.Content.ReadAsStringAsync();
        dynamic json = JsonConvert.DeserializeObject(responseData);
       
        return JsonConvert.DeserializeObject<Dictionary<string, double>>(JsonConvert.SerializeObject(json?.rates));
       
    }
}