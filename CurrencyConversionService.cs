using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ExchangeCurrencyAPIService;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class CurrencyConversionService
{
    private readonly HttpClient _httpClient;
 

    public CurrencyConversionService(HttpClient httpClient)
    {
        _httpClient = httpClient;
       
    }

    //rishika
    public async Task<Dictionary<string, double>> GetExchangeRatesAsync(string baseCurrency = "NOK")
    {
        string url = $"{AppConfiguration.Base_Url}?access_key={AppConfiguration.Api_Key}";
    
        var response = await _httpClient.GetStringAsync(url);
        var exchangeRates = JsonConvert.DeserializeObject<FixerApiResponse>(response);
        return exchangeRates.Rates;
    }


    public async Task<double> ConvertCurrencyAsync(string first, string target, double amount)
    {
        var rates = await GetExchangeRatesAsync();

        if (!rates.ContainsKey(first) || !rates.ContainsKey(target))
        {
            throw new ArgumentException("Please enter a valid currency code.");
        }

        double firstToBaseRate = rates[first];
        double targetToBaseRate = rates[target];

        double convertedAmount = (amount / firstToBaseRate) * targetToBaseRate;
        return convertedAmount;
    }
}

public class FixerApiResponse
{
    public DateTime Date { get; set; }
    public Dictionary<string, double> Rates { get; set; }
}
