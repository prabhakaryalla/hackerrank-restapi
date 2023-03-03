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

namespace HackerRank
{
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
            int totalCount = 0;
            int totalpulse = 0;
            using (var client1 = new HttpClient())
            {
                var result = client1.GetAsync(url).Result;
                var response = result.Content.ReadAsStringAsync().Result;
                var json = System.Text.Json.JsonSerializer.Deserialize<ODataResponse<MedicalReponse>>(response);
                totalpages = json.total_pages;
            }
            using var client = new HttpClient();
            for (int i =1; i<= totalpages; i++)
            {
                var result = client.GetAsync(url + $"?page={i}").Result;
                var response = result.Content.ReadAsStringAsync().Result;
                var json = System.Text.Json.JsonSerializer.Deserialize<ODataResponse<MedicalReponse>>(response);
                var docmedicalReports = (from s in json.data where s.diagnosis.name == diagnosisName && s.doctor.id == doctorId select s)?.ToList();
                totalCount += docmedicalReports.Count;
                totalpulse += docmedicalReports.Sum(x => x.vitals.pulse);
            }

            return (totalpulse/totalCount);
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
