using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

public partial class CurrencyConverter
{
    private static Dictionary<string, double> exchangeRates;

    public static async Task Main()
    {

        try
        {
            Console.Write("Enter the source currency code: ");
            string sourceCurrecny = Console.ReadLine().ToUpper();

            Console.Write("Enter the target currency code: ");
            string targetCurrency = Console.ReadLine()?.ToUpper();


            Console.Write("Enter the amount: ");
            double amount = Convert.ToDouble(Console.ReadLine());

            Console.Write("Do you want exchange rate by any specific Date: ");
            string answer = Console.ReadLine();
            string date = "latest";
            if (answer == "YES")
            {
                Console.Write("Enter the Data in Formate yyyy-mm--dd: ");
                date = Console.ReadLine();

            }

            Console.WriteLine("Fetch latest exchange rates-");
            CurrencyExchnageAPISercvice apiService = new CurrencyExchnageAPISercvice();

            var exchangeRates = await apiService.GetCurrencyExchangeRates(date);

            double result = ConvertCurrency(sourceCurrecny, targetCurrency, amount);

            Console.WriteLine($"\n{amount} {sourceCurrecny} is equal to {result:F2} {targetCurrency}.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"\nError: {e.Message}");
        }
    }

    public static double ConvertCurrency(string sourceCurrecny, string targetCurrency, double amount)
    {
        if (!exchangeRates.ContainsKey(sourceCurrecny))
        {
            throw new ArgumentException($"Invalid currency code: {sourceCurrecny}");
        }
        if (!exchangeRates.ContainsKey(targetCurrency))
        {
            throw new ArgumentException($"Invalid currency code: {targetCurrency}");
        }

        // Convert from the source currency to EUR, then to the target currency
        double convertedAmount = (amount / exchangeRates[sourceCurrecny]) * exchangeRates[targetCurrency];
        return convertedAmount;
    }

}