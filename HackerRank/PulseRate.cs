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
    public static class PulseRate
    {
        /*
    * Complete the 'pulseRate' function below.
    *
    * The function is expected to return an INTEGER.
    * The function accepts following parameters:
    *  1. STRING diagnosisName
    *  2. INTEGER doctorId
    * API URL: https://jsonmock.hackerrank.com/api/medical_records?page={page_no}
    */

        public static int pulseRate(string diagnosisName, int doctorId)
        {
            var url = "https://jsonmock.hackerrank.com/api/medical_records";
            int totalpages = 0;
            ConcurrentBag<int> totalCount = new ConcurrentBag<int>();
            ConcurrentBag<int> totalpulse = new ConcurrentBag<int>();
            using var client = new HttpClient();
            var res1 = client.GetAsync(url).Result;
            var response1 = res1.Content.ReadAsStringAsync().Result;
            var json1 = System.Text.Json.JsonSerializer.Deserialize<ODataResponseWithoutData>(response1);
            totalpages = json1.total_pages;

            Parallel.For(1, totalpages + 1, new ParallelOptions() { MaxDegreeOfParallelism = 12 }, i =>
            {
                var result = client.GetAsync(url + $"?page={i}").Result;
                var response = result.Content.ReadAsStringAsync().Result;
                var json = System.Text.Json.JsonSerializer.Deserialize<ODataResponse<MedicalReponse>>(response);
                var docmedicalReports = (from s in json.data where s.diagnosis.name == diagnosisName && s.doctor.id == doctorId select s)?.ToList();
                totalCount.Add(docmedicalReports.Count);
                totalpulse.Add(docmedicalReports.Sum(x => x.vitals.pulse));               
            });

            return (totalpulse.Sum() / totalCount.Sum());
        }
    }

    public class MedicalReponse
    {
        public Diagnosis diagnosis { get; set; }
        public Doctor doctor { get; set; }
        public Vitals vitals { get; set; }  
        
    }

    public class Diagnosis
    {
        public string name { get; set; }  
        public int id { get; set; }
    }

    public class Vitals
    {
        public int pulse { get; set; }
        public decimal bodyTemperature { get; set; }
    }

    public class Doctor
    {
        public int id { get; set; }
        public string name { get; set; }
    }

}
