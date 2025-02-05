using ExchangeCurrencyAPIService;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
    public async Task<Dictionary<string, double>> GetExchangeRatesAsync(string baseCurrency, string? date)
    {
        //string url = $"{AppConfiguration.Base_Url}{date}?access_key={AppConfiguration.Api_Key}";
        string url = date is not null
            ? $"{AppConfiguration.Base_Url}{date}?access_key={AppConfiguration.Api_Key}"
            : $"{AppConfiguration.Base_Url}latest?access_key={AppConfiguration.Api_Key}"; 
        Console.WriteLine("url for api is " + url);
        var response = await _httpClient.GetStringAsync(url);
        var exchangeRates = JsonConvert.DeserializeObject<FixerApiResponse>(response);
        Console.WriteLine("respone for api is " + response);
        Console.WriteLine("exchangerates for api is " + exchangeRates);
        return exchangeRates.Rates;
    }

    // Convert currency using the latest rates
    // public async Task<double> ConvertCurrencyAsync(string first, string target, double amount, string date="latest")
    public async Task<double> ConvertCurrencyAsync(string first, string target, double amount, string? date)
    {
     
        if (first.Length > 3 || target.Length > 3)
        {
            throw new ArgumentException("Invalid currency codes");
        }
        //var rates = await GetExchangeRatesAsync(first);
        if (date == null)
        {
            date = "latest";
        }
        var rates = await GetExchangeRatesAsync(first, date);
       

       // var rates = await GetExchangeRatesAsync(first);

        if (!rates.ContainsKey(first) || !rates.ContainsKey(target))
        {
            throw new ArgumentException("Invalid currency codes provided.");
        }

        double sourceToBaseRate = rates[first];
        double targetToBaseRate = rates[target];

        double convertedAmount = (amount / sourceToBaseRate) * targetToBaseRate;
        return convertedAmount;
    }


    // Convert currency using the latest rates for a specific date
    //public async Task<double> ConvertCurrencyAsyncForDate(string first, string target, double amount, string date)
    //{
    //    var rates = await GetExchangeRatesAsync(first, date);

    //    if (!rates.ContainsKey(first) || !rates.ContainsKey(target))
    //    {
    //        throw new ArgumentException("Invalid currency codes provided.");
    //    }

    //    double sourceToBaseRate = rates[first];
    //    double targetToBaseRate = rates[target];
    //    double convertedAmount = (amount / sourceToBaseRate) * targetToBaseRate;
    //    return convertedAmount;
    //}
}

public class FixerApiResponse
{
    public DateTime Date { get; set; }
    public Dictionary<string, double> Rates { get; set; }
}

