using System;
namespace ExchangeCurrencyAPIService
{
    public class AppConfig
    {
        public const string Api_Key = "20b8a5d34f31e82e810157230e3a7d53";
        public const string Base_Url = "http://data.fixer.io/api/latest?";
        public const string SqlServer = "currencyexchangedata.database.windows.net";
        public const string DBConnString = "Server=tcp:currencyexchangedata.database.windows.net,1433;Initial Catalog=CurrencyStorage;Persist Security Info=False;User ID=rishikadata;Password=Rishi@2025;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
    }
}
