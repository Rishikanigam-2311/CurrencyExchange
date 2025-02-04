using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

public class CurrencyExchangeCalculator
{
    public static async Task Main()
    {

        try
        {
            Console.Write("Welcome to Currency Exchange calculator! \n Enter the first currency code: ");
            string first = Console.ReadLine().ToUpper();
            if (first.Length > 3)
            {
                throw new ArgumentException("Invalid currency code.");
            }

            Console.Write("Enter the target currency code: ");
            string target = Console.ReadLine().ToUpper();
            if (target.Length > 3)
            {
                throw new ArgumentException("Invalid currency code.");
            }

            Console.Write("Enter the amount: ");
            double amount = Convert.ToDouble(Console.ReadLine());

            Console.Write("Do you want to fetch the exchange rate for any specific Date: ");
            string answer = Console.ReadLine().ToUpper();
            string date = "latest";
            if (answer == "YES")
            {
                Console.Write("Enter the Data in format yyyy-MM-dd: ");
                date = Console.ReadLine();

            }

            Console.WriteLine("Fetching the latest exchange rates: ");
            CurrencyExchnageAPISercvice apiService = new CurrencyExchnageAPISercvice();

            var exchangeRates = await apiService.GetCurrencyExchangeRates(date);

            double result = ConvertCurrency(first, target, amount, exchangeRates);

            Console.WriteLine($"\n{amount} {first} is equal to {result:F2} in {target}.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"\nError: {e.Message}");
        }
    }

    public static double ConvertCurrency(string first, string target, double amount, Dictionary<string, double> rates)
    {
        if (!rates.ContainsKey(first))
        {
            throw new ArgumentException($"Invalid currency code: {first}");
        }
        if (!rates.ContainsKey(target))
        {
            throw new ArgumentException($"Invalid currency code: {target}");
        }

        double convertedAmount1 = (amount / rates[first]);
        double convertedAmount2 = convertedAmount1 * rates[target];
        return convertedAmount2;
    }

}