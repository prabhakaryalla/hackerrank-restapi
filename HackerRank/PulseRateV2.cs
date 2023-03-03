using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;

namespace HackerRank
{
    public static class PulseRateV2
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
            int totalPulseRate = 0;
            int pageNumber = 1;
            int totalPages = 1;
            decimal counter = 0;
            decimal pulses = 0;

            //TODO: Need to change below implemtation with concurrent approach if this fails due to the page should be loaded under 3 sec
            //Please refer to BodyTemperature class for the implementation

            using (var httpClient = new HttpClient())
            {
                while (pageNumber <= totalPages)
                {
                    string url = $"https://jsonmock.hackerrank.com/api/medical_records?page={pageNumber}";
                    var res = httpClient.GetAsync(url).Result;
                    if (res.IsSuccessStatusCode)
                    {
                        var content = res.Content.ReadAsStringAsync().Result;
                        var jsonData = JObject.Parse(content);
                        if (pageNumber == 1)
                        {
                            totalPages = (int)jsonData["total_pages"];
                        }
                        var records = jsonData["data"];


                        //var result = (from s in records
                        //              where JObject.Parse(s.ToString()) != null
                        //              && Convert.ToInt32(JObject.Parse(JObject.Parse(s.ToString())["doctor"].ToString())["id"]) == doctorId
                        //              && JObject.Parse(JObject.Parse(s.ToString())["diagnosis"].ToString())["name"].ToString() == diagnosisName
                        //              select JObject.Parse(JObject.Parse(s.ToString())["vitals"].ToString()))?.ToList();
                        //pulses += result.Sum(s => Convert.ToInt32(s["pulse"]));
                        //counter += result.Count();
                        foreach (var record in records)
                        {
                            var pData = JObject.Parse(record.ToString());
                            if (pData != null)
                            {
                                var doctor = JObject.Parse(pData["doctor"].ToString());
                                var diagnosis = JObject.Parse(pData["diagnosis"].ToString());
                                if (Convert.ToInt32(doctor["id"]) == doctorId && diagnosis["name"].ToString() == diagnosisName)
                                {
                                    if (pData.ContainsKey("vitals"))
                                    {
                                        var vitals = JObject.Parse(pData["vitals"].ToString());
                                        if (vitals != null && vitals.ContainsKey("pulse") && vitals["pulse"] != null)
                                        {
                                            pulses = pulses + Convert.ToInt32(vitals["pulse"]);
                                            counter = counter + 1;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    pageNumber++;
                }
            }

            totalPulseRate = Convert.ToInt32(Math.Floor(pulses / counter));
            return totalPulseRate;
        }
    }
}
