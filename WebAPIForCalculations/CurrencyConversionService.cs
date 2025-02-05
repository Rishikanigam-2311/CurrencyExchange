using ExchangeCurrencyAPIService;
using Newtonsoft.Json;

public class CurrencyConversionService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _baseUrl = "https://api.apilayer.com/fixer";

    public object date { get; private set; }

    // Constructor to initialize HttpClient and API key
    public CurrencyConversionService(IConfiguration configuration, HttpClient httpClient)
    {
        _httpClient = httpClient;
        _apiKey = configuration["FixerApiKey"];
    }

    // Fetch the latest exchange rates from the API
    public async Task<Dictionary<string, double>> GetExchangeRatesAsync(string baseCurrency)
    {
        string url = $"{AppConfiguration.Base_Url}access_key={AppConfiguration.Api_Key}";
        Console.WriteLine("url for api is " + url);
        var response = await _httpClient.GetStringAsync(url);
        var exchangeRates = JsonConvert.DeserializeObject<FixerApiResponse>(response);
        Console.WriteLine("respone for api is " + response);
        Console.WriteLine("exchangerates for api is " + exchangeRates);
        return exchangeRates.Rates;
    }

    // Convert currency using the latest rates
    // public async Task<double> ConvertCurrencyAsync(string sourceCurrency, string targetCurrency, double amount, string date="latest")
    public async Task<double> ConvertCurrencyAsync(string sourceCurrency, string targetCurrency, double amount)

    {
        var rates = await GetExchangeRatesAsync(sourceCurrency);
        // var rates = await GetExchangeRatesAsync(sourceCurrency, date);

        if (!rates.ContainsKey(sourceCurrency) || !rates.ContainsKey(targetCurrency))
        {
            throw new ArgumentException("Invalid currency codes provided.");
        }

        double sourceToBaseRate = rates[sourceCurrency];
        double targetToBaseRate = rates[targetCurrency];

        double convertedAmount = (amount / sourceToBaseRate) * targetToBaseRate;
        return convertedAmount;
    }


    // Convert currency using the latest rates for a specific date
    //public async Task<double> ConvertCurrencyAsyncForDate(string sourceCurrency, string targetCurrency, double amount, string date)
    //{
    //    var rates = await GetExchangeRatesAsync(sourceCurrency, date);

    //    if (!rates.ContainsKey(sourceCurrency) || !rates.ContainsKey(targetCurrency))
    //    {
    //        throw new ArgumentException("Invalid currency codes provided.");
    //    }

    //    double sourceToBaseRate = rates[sourceCurrency];
    //    double targetToBaseRate = rates[targetCurrency];
    //    double convertedAmount = (amount / sourceToBaseRate) * targetToBaseRate;
    //    return convertedAmount;
    //}
}

public class FixerApiResponse
{
    public DateTime Date { get; set; }
    public Dictionary<string, double> Rates { get; set; }
}

