using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;

namespace HackerRank
{
    //This is verified and working fine in hackerrank

    public static class PhoneNumber
    {
        /*
     * Complete the 'getPhoneNumbers' function below.
     *
     * The function is expected to return a STRING.
     * The function accepts following parameters:
     *  1. STRING country
     *  2. STRING phoneNumber
     * API URL: https://jsonmock.hackerrank.com/api/countries?name=<country>
     */

        public static string getPhoneNumbers(string country, string phoneNumber)
        {
            string apiUrl = $"https://jsonmock.hackerrank.com/api/countries?name={country}";
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(apiUrl);
                var response = httpClient.GetAsync(apiUrl).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    if (data == null)
                    {
                        return "-1";
                    }

                    var serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(CountriesPage));
                    using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(data)))
                    {
                        var countries = (CountriesPage)serializer.ReadObject(stream);
                        if (countries == null || countries?.data == null || countries?.data?.Count() == 0)
                        {
                            return "-1";
                        }
                        var countryCodes = countries.data.FirstOrDefault()?.callingCodes;
                        int latestCountryCode = 0;
                        foreach (var code in countryCodes)
                        {
                            int convertedCode = Convert.ToInt32(code);
                            if (convertedCode > latestCountryCode)
                            {
                                latestCountryCode = convertedCode;
                            }
                        }
                        return "+" + latestCountryCode + " " + phoneNumber;
                    }
                }
                else
                {
                    return "-1";
                }
            }
        }

    }


    [DataContractAttribute]
    class CountriesPage
    {
        [DataMemberAttribute]
        public int page { get; set; }
        [DataMemberAttribute]
        public int per_page { get; set; }
        [DataMemberAttribute]
        public int total { get; set; }
        [DataMemberAttribute]
        public int total_pages { get; set; }
        [DataMemberAttribute]
        public List<Country> data { get; set; }

    }
    [DataContractAttribute]
    class Country
    {
        [DataMemberAttribute]
        public List<string> callingCodes { get; set; }
    }

}
