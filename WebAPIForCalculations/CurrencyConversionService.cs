using ExchangeCurrencyAPIService;
using Newtonsoft.Json;

public class CurrencyConversionService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public CurrencyConversionService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["FixerApiKey"];
    }

    public async Task<double> ConvertCurrencyAsync(string first, string target, double amount, string? date)
    {

        if (first.Length > 3 || target.Length > 3)
        {
            throw new ArgumentException("Invalid currency codes");
        }
        if (date == null)
        {
            date = "latest";
        }
        var rates = await GetExchangeRatesAsync(first, date);

        if (!rates.ContainsKey(first) || !rates.ContainsKey(target))
        {
            throw new ArgumentException("Invalid currency codes provided.");
        }

        double sourceToBaseRate = rates[first];
        double targetToBaseRate = rates[target];

        double convertedAmount = (amount / sourceToBaseRate) * targetToBaseRate;
        return convertedAmount;
    }
    public async Task<Dictionary<string, double>> GetExchangeRatesAsync(string baseCurrency, string? date)
    {
        string url = date is not null
            ? $"{AppConfiguration.Base_Url}{date}?access_key={AppConfiguration.Api_Key}"
            : $"{AppConfiguration.Base_Url}latest?access_key={AppConfiguration.Api_Key}";

        var response = await _httpClient.GetStringAsync(url);

        if (response == null)
        {
            throw new Exception("Failed to fetch exchange rates.");
        }

        var exchangeRates = JsonConvert.DeserializeObject<FixerApiResponse>(response);
        return exchangeRates.Rates;
    }

}

public class FixerApiResponse
{
    public Dictionary<string, double> Rates { get; set; }
}

