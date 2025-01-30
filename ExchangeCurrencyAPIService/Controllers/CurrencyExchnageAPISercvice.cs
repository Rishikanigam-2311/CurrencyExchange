using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ExchangeCurrencyAPIService;
[ApiController]
[Route("[Controllers]")]
public class CurrencyExchnageAPISercvice : ControllerBase
{
    public async Task<Dictionary<string, decimal>> GetCurrencyExchangeRates(string date)
    {
        AppConfiguration config = new AppConfiguration();
        string url = $"{config?.Routes?.BaseURL}{date}?access_key={config?.Routes?.API_KEY}";
        Console.WriteLine(url);

        using HttpClient client = new();

        HttpResponseMessage response = await client.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Unable to fetch exhange rate: {response.StatusCode}");
        }

        string responseData = await response.Content.ReadAsStringAsync();
        //dynamic json = JsonConvert.DeserializeObject(responseData);



        // Deserialize rates into a dictionary
        //return JsonConvert.DeserializeObject<Dictionary<string, double>>(JsonConvert.SerializeObject(json?.rates));
        // Deserialize rates into a dictionary
        return JsonConvert.DeserializeObject<Dictionary<string, decimal>>(responseData);
    }
}