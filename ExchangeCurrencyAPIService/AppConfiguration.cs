namespace ExchangeCurrencyAPIService
{
    public class AppConfiguration
    {

        public Routes Routes { get; set; }
        public ConnectionString connectionString { get; set; }


    }

    public class Routes
    {
        public string API_KEY { get; set; }
        public string BaseURL { get; set; }
    }

    public class ConnectionString
    {
        public string db_ConnectionString { get; set; }
    }
}
