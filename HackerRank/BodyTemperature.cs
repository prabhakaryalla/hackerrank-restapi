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
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace HackerRank
{
    public static class BodyTemperature
    {
        /*
    * Complete the 'bodyTemperature' function below.
    *
    * The function is expected to return an INTEGER_ARRAY.
    * The function accepts following parameters:
    *  1. STRING doctorName
    *  2. INTEGER diagnosisId
    * API URL: https://jsonmock.hackerrank.com/api/medical_records?page={page_no}
    */

        public static List<int> bodyTemperature(string doctorName, int diagnosisId)
        {
            var url = "https://jsonmock.hackerrank.com/api/medical_records";
            ConcurrentBag<decimal> res = new ConcurrentBag<decimal>();
            int totalpages = 0;
            using var client = new HttpClient();
            var result = client.GetAsync(url).Result;
            var response = result.Content.ReadAsStringAsync().Result;
            var json = System.Text.Json.JsonSerializer.Deserialize<ODataResponseWithoutData>(response);
            totalpages = json.total_pages;

            Parallel.For(1, totalpages + 1, new ParallelOptions() { MaxDegreeOfParallelism = 12 }, i =>
            {
                var pagematchesurl = "https://jsonmock.hackerrank.com/api/medical_records?page=" + i;
                var result = client.GetAsync(pagematchesurl).Result;
                var response = result.Content.ReadAsStringAsync().Result;
                var json = System.Text.Json.JsonSerializer.Deserialize<ODataResponse<MedicalReponse>>(response);
                var filtered = (from s in json.data where s.doctor.name.ToLower() == doctorName.ToLower() && s.diagnosis.id == diagnosisId select s);
                foreach(var item in filtered )
                {
                    res.Add(item.vitals.bodyTemperature);
                }
            });

            return new List<int>() { Convert.ToInt32(Math.Floor(res.Min())), Convert.ToInt32(Math.Floor(res.Max())) };
        }
    }

    //Note: Please refer to the class from PulseRate.cs (MedicalReponse,Diagnosis,Vitals,Doctor) which are reffered in this class

    public class ODataResponseWithoutData
    {
        public int page { get; set; }
        public int per_page { get; set; }
        public int total_pages { get; set; }
    }
}
