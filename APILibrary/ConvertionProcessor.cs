using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace APILibrary
{

    // This is where the API magic happens. Mostly.
    public class ConvertionProcessor
    {
        // GET method for all available currencies and their exchange rates.
        public async Task<string[]> LoadCurrencies(APIHelper apiHelper)
        {
            string url = $"/latest";

            using (HttpResponseMessage response = await apiHelper.apiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    string rates = await response.Content.ReadAsStringAsync();

                    // Since I'm a lazy bum who didn't want to figure out how to procedurally cache
                    // json data from an API, I had to extract the information I wanted manually.
                    var charsToRemove = new string[] { "{", "}", "\"", ":", "." };
                    foreach (var c in charsToRemove)
                    {
                        rates = rates.Replace(c, string.Empty);
                    }

                    rates = Regex.Replace(rates, @"[\d-]", string.Empty);
                    rates = Regex.Replace(rates, "[a-z]", string.Empty);

                    string[] ratesList = new string[rates.Length];
                    ratesList = rates.Split(',');

                    return ratesList;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<string> ConvertCurrency (APIHelper apiHelper, string currency_1, string currency_2, float amount)
        {
            string url = $"/latest?base={currency_1}&symbols={currency_1},{currency_2}";

            using (HttpResponseMessage response = await apiHelper.apiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    // Once again, due to me being a lazy bum who didn't wanna google how to procedurally
                    // cache json data without hard coding a class for the exchange rates, which in
                    // retrospect would've likely been way better solution, I had to extract the data I wanted
                    // by myself.
                    string convertionData = await response.Content.ReadAsStringAsync();
                    var charsToRemove = new string[] { "{", "}", "\"", ":", "," };


                    foreach (var c in charsToRemove)
                    {
                        convertionData = convertionData.Replace(c, string.Empty);
                    }

                    convertionData = Regex.Replace(convertionData, "[a-z]", string.Empty);
                    convertionData = Regex.Replace(convertionData, "[A-Z]", string.Empty);

                    // We have to remove the last ten symbols in the string due to these being a date.
                    // Stupid solution, this would all have been avoided if I only properly cached the data
                    // in an object like you're supposed to do. What was I thinking?
                    convertionData = convertionData.Remove(convertionData.Length-10);

                    return convertionData;
                }

                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
