using Microsoft.Azure.Mobile.Server.Config;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
namespace ExchangeCurrencyAPIService;

public class CurrencyUpdaterService
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public CurrencyUpdaterService(IConfiguration configuration)
    {
        _configuration = configuration;
        _httpClient = new HttpClient();
    }
    //1-c- DB creation and batch job creation
    public async Task FetchAndStoreExchangeRatesAsync()
    {
        try
        {
            string url = $"{AppConfig.Base_Url}access_key={AppConfig.Api_Key}";
           // var url = "http://data.fixer.io/api/latest?access_key=161c036cebf156a83f79dd0c00cfe6e1";
            Console.WriteLine(url);
            var response = await _httpClient.GetStringAsync(url);
            var exchangeRates = JsonConvert.DeserializeObject<ExchangeRatesResponse>(response);

            if (exchangeRates?.Rates != null)
            {
                foreach (var rate in exchangeRates.Rates)
                {
                    await StoreCurrencyRateInDatabaseAsync(rate.Key, DateTime.UtcNow, rate.Value);
                }
            }
            else
            {
                Console.WriteLine("Failed to retrieve valid exchange rates.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching exchange rates: {ex.Message}");
        }
    }

    private async Task StoreCurrencyRateInDatabaseAsync(string currencyCode, DateTime date, decimal rate)
    {
        var connectionString = _configuration.GetConnectionString("SqlServer");

        using (var connection = new SqlConnection(connectionString))
        {
            await connection.OpenAsync();


            using (var command = new SqlCommand("SP_CurrencyUpdates", connection))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CurrencyCode", currencyCode);
                command.Parameters.AddWithValue("@Date", date);
                command.Parameters.AddWithValue("@Rate", rate);

               await command.ExecuteNonQueryAsync();
            }

            Console.WriteLine($"Successfully inserted exchange rate for {currencyCode}.");
        }
    }
}

public class ExchangeRatesResponse
{
    public string Base { get; set; }
    public Dictionary<string, decimal> Rates { get; set; }
}
