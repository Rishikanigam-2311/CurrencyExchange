using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

public class CurrencyExchangeCalculator
{
    public static async Task Main()
    {
        DateTime currentDate = DateTime.Now.Date;
        //Point 1-a - Console app for currency converter
        try
        {
            Console.Write("Welcome to Currency Exchange calculator! \nEnter the first currency code: ");
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

            Console.Write("Enter the Amount: ");
            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input)) 
            {
                throw new ArgumentException("Amount is required.");
            }

            double amount = Convert.ToDouble(input); 

            //Point 1-b - Asking user if data is required for any specific date
            Console.Write("Want to fetch exchange rate for a specific Date? ");
            string answer = Console.ReadLine().ToUpper();
            string date = "latest";

            if (answer == "YES")
            {
                Console.Write("Enter the Date: ");
                date = Console.ReadLine();

                if (DateTime.TryParse(date, out DateTime inputDate))
                {
                    if (inputDate > currentDate)
                    {
                        {
                            throw new ArgumentException($"Invalid future date provided");
                        }
                    }
                   
                }

            }
            CurrencyExchnageAPISercvice apiService = new CurrencyExchnageAPISercvice();

            var exchangeRates = await apiService.GetCurrencyExchangeRates(date);

            double result = ConvertCurrency(first, target, amount, exchangeRates);

            Console.WriteLine($"\n{amount} {first} is equal to {result} in {target}.");
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