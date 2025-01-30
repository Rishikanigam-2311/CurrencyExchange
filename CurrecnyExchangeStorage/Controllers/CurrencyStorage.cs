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
            AppConfiguration config = new AppConfiguration();
            Console.WriteLine("Fetching latest exchange rates");

            var exchangeRates = await apiService.GetCurrencyExchangeRates(null);
            SaveRatesToDatabaseAsync(exchangeRates, DateTime.UtcNow, config.connectionString.db_ConnectionString);
            Console.WriteLine("Exchange rates updated successfully.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"\nError: {e.Message}");
        }       
    }

    private static void SaveRatesToDatabaseAsync(Dictionary<string, decimal> rates, DateTime date, string Database_ConnectionString)
    {
        using SqlConnection conn = new SqlConnection(Database_ConnectionString);
        conn.Open();

        foreach (var rate in rates)
        {
            string query = "INSERT INTO Users (Id, Name, Email) VALUES (@Id, @Name, @Email)";
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@CurrencyCode", rate.Key);
            cmd.Parameters.AddWithValue("@Rate", rate.Value);
            cmd.Parameters.AddWithValue("@Date", date);
            cmd.ExecuteNonQuery();
        }
    }
}