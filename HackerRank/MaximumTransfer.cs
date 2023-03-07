using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Text;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace HackerRank
{

    //This is verified and working fine in hackerrank

    public static class MaximumTransfer
    {

        /*
     * Complete the 'maximumTransfer' function below.
     *
     * The function is expected to return a STRING_ARRAY.
     * The function accepts following parameters:
     *  1. STRING name
     *  2. STRING city
     * API URL: https://jsonmock.hackerrank.com/api/transactions?page={page_no}
     */

        public static List<string> maximumTransfer(string name, string city)
        {
            var totalpages = 0;
            var url = "https://jsonmock.hackerrank.com/api/transactions";
            ConcurrentBag<float> debit = new ConcurrentBag<float>();
            ConcurrentBag<float> credit = new ConcurrentBag<float>();

            using var client = new HttpClient();
            var res1 = client.GetAsync(url).Result;
            var response1 = res1.Content.ReadAsStringAsync().Result;

            var serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(Transactions));
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(response1)))
            {
                var transactions = (Transactions)serializer.ReadObject(stream);

                totalpages = transactions.total_pages;
            }

            Parallel.For(1, totalpages + 1, new ParallelOptions() { MaxDegreeOfParallelism = 12 }, i =>
            {
                var result = client.GetAsync(url + $"?page={i}").Result;

                var response = result.Content.ReadAsStringAsync().Result;
                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(response)))
                {
                    var json = (Transactions)serializer.ReadObject(stream);

                    var records = (from s in json.data where s.userName == name && s.location.city == city select s)?.ToList();
                    foreach (var transaction in records)
                    {

                        var style = NumberStyles.AllowCurrencySymbol | NumberStyles.Integer | NumberStyles.AllowThousands;

                        float.TryParse(transaction.amount.Replace(",", string.Empty).Replace("$", string.Empty), out float amountconvert);

                        if (transaction.txnType == "debit")
                        {
                            debit.Add(amountconvert);
                        }
                        else if (transaction.txnType == "credit")
                        {
                            credit.Add(amountconvert);
                        }
                    }
                }
            });

            return new List<string> { $"${credit.Max():n}", $"${debit.Max():n}" };

        }

    }


    [DataContractAttribute]
    class Transactions
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
        public List<User> data { get; set; }

    }
    [DataContractAttribute]
    class User
    {

        [DataMemberAttribute]
        public int id { get; set; }
        [DataMemberAttribute]
        public int userId { get; set; }
        [DataMemberAttribute]
        public string userName { get; set; }
        [DataMemberAttribute]
        public string timestamp { get; set; }
        [DataMemberAttribute]
        public string txnType { get; set; }
        [DataMemberAttribute]
        public string amount { get; set; }
        [DataMemberAttribute]

        public LocationData location { get; set; }
    }

    [DataContractAttribute]
    class LocationData
    {

        [DataMemberAttribute]
        public int id { get; set; }
        [DataMemberAttribute]
        public string address { get; set; }
        [DataMemberAttribute]
        public string city { get; set; }
        [DataMemberAttribute]
        public string zipCode { get; set; }

    }
}
