using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace HackerRank
{
    public static class PhoneNumber
    {
        public static string getPhoneNumbers(string country, string phoneNumber)
        {
            string result = "-1";
            using (var client = new HttpClient())
            {
                string url = $"https://jsonmock.hackerrank.com/api/countries?name={country}";
                HttpResponseMessage response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    var jsonData = JObject.Parse(content);
                    if (jsonData != null && jsonData.ContainsKey("data") && jsonData["data"] != null && jsonData["data"].Count() > 0)
                    {
                        var data = JObject.Parse(jsonData["data"][0].ToString());
                        if (data != null && data.ContainsKey("callingCodes") && data["callingCodes"] != null && data["callingCodes"].Count() > 0)
                        {
                            var callingCodes = data["callingCodes"];
                            result = $"+{callingCodes.Last.ToString()} {phoneNumber}";
                        }
                    }
                }
            }

            return result;
        }

    }
}
