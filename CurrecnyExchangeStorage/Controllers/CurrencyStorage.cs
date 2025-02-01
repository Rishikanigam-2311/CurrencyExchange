using ExchangeCurrencyAPIService;
using Microsoft.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;
class DBBatchJob
{
    public static async Task Main()
    {
        Console.WriteLine("Starting Batch Job");

        try
        {

            Console.WriteLine("Data successfully saved to Azure SQL Database!");

            CurrencyExchnageAPISercvice apiService = new CurrencyExchnageAPISercvice();

            Console.WriteLine("Fetching latest exchange rates");

            var exchangeRates = await apiService.GetCurrencyExchangeRates(null);
            SaveRatesToDatabaseAsync(exchangeRates, DateTime.UtcNow, AppConfiguration.DBConnString);
            Console.WriteLine("Exchange rates updated successfully.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"\nError: {e.Message}");
        }
    }

    private static void SaveRatesToDatabaseAsync(Dictionary<string, double> rates, DateTime date, string Database_ConnectionString)
    {
        using SqlConnection conn = new SqlConnection(Database_ConnectionString);
        conn.Open();

        foreach (var rate in rates)
        {
            string query = "INSERT INTO CurrencyUpdates (Id, CurrencyCode, Rate, Date) VALUES (@Id, @CurrencyCode, @Rate, @Date)";
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@CurrencyCode", rate.Key);
            cmd.Parameters.AddWithValue("@Rate", rate.Value);
            cmd.Parameters.AddWithValue("@Date", date);
            cmd.ExecuteNonQuery();
        }
    }


        private readonly TimeSpan _checkInterval = TimeSpan.FromHours(1);

        protected async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var currentTime = DateTime.Now.TimeOfDay;

            foreach (var scheduledTime in SchedulerTime)
            {
                if (currentTime >= scheduledTime && currentTime < scheduledTime.Add(_checkInterval))
                {
                    await ExecuteScheduledTask(scheduledTime);
                }
            }

        }

        private Task ExecuteScheduledTask(TimeSpan scheduledTime)
        {
            double result = DBBatchJob();
            Console.WriteLine($"Task executed at {scheduledTime}");
            return Task.CompletedTask;
        }
        private bool _taskExecutedToday = false;
    }
}


//using ExchangeCurrencyAPIService;
//using Microsoft.Azure.WebJobs;
//using Microsoft.Data.SqlClient;
//using Newtonsoft.Json;
//using static System.Runtime.InteropServices.JavaScript.JSType;

//public class DBBatchJob
//{
//    private static readonly HttpClient client = new HttpClient();
//    [FunctionName("FetchExchangeRates")]
//    public static async Task Run(
//        [TimerTrigger("0 0 9 * * *")] TimerInfo timer,  
//        ILogger log)
//    {
//        try
//        {  
//            string url = $"{AppConfiguration.Base_Url}latest?access_key={AppConfiguration.Api_Key}";

//            var response = await client.GetStringAsync(url);

//            // Deserialize the JSON response to get exchange rates
//            var exchangeRates = JsonConvert.DeserializeObject<ExchangeRatesResponse>(response);

//            // Log the operation
//            log.LogInformation("Fetched exchange rates from Fixer.io");

//            // Store the exchange rates in SQL Server
//            if (exchangeRates?.Rates != null)
//            {
//                await SaveExchangeRatesToDatabase(exchangeRates.Rates, log);
//            }
//            else
//            {
//                log.LogWarning("No exchange rates data found in response.");
//            }
//        }
//        catch (Exception ex)
//        {
//            log.LogError($"Error occurred: {ex.Message}");
//        }
//    }

//    private static async Task SaveExchangeRatesToDatabase(Dictionary<string, decimal> rates, ILogger log)
//    {
//        string connectionString = AppConfiguration.DBConnString;

//        try
//        {
//            using (SqlConnection connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString))
//            {
//                await connection.OpenAsync();

//                foreach (var rate in rates)
//                {
//                    string query = @"
//                        INSERT INTO ExchangeRates (BaseCurrency, TargetCurrency, Rate, Date)
//                        VALUES (@BaseCurrency, @TargetCurrency, @Rate, @Date)";

//                    using (SqlCommand cmd = new SqlCommand(query, connection))
//                    {
//                        // Set the parameters for each currency pair
//                        cmd.Parameters.AddWithValue("@BaseCurrency", "USD");
//                        cmd.Parameters.AddWithValue("@TargetCurrency", rate.Key);
//                        cmd.Parameters.AddWithValue("@Rate", rate.Value);
//                        cmd.Parameters.AddWithValue("@Date", DateTime.UtcNow);  // Use the current date as the record date

//                        // Execute the insert command
//                        await cmd.ExecuteNonQueryAsync();
//                    }
//                }

//                log.LogInformation("Exchange rates successfully stored in database.");
//            }
//        }
//        catch (Exception ex)
//        {
//            log.LogError($"Database operation failed: {ex.Message}");
//        }
//    }
//}

//// Define a class to hold the JSON structure from the Fixer.io API response
//public class ExchangeRatesResponse
//{
//    public string Base { get; set; }
//    public Dictionary<string, decimal> Rates { get; set; }
}
