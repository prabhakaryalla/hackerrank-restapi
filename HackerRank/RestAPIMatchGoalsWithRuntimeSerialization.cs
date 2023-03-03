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
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization.Formatters.Binary;

namespace HackerRank
{
   static class RestAPIMatchGoalsWithRuntimeSerialization
    {
        [DataContract]
        public class ODataResponse<T>
        {
            [DataMember]
            public int page { get; set; }
            [DataMember]
            public int per_page { get; set; }
            [DataMember]
            public int total_pages { get; set; }
            [DataMember]
            public T[] data { get; set; }
        }

        [DataContract]
        public class CompetationRes
        {
            [DataMember]
            public string winner { get; set; }
        }

        [DataContract]
        public class MatchResonse
        {
            [DataMember]
            public string team1 { get; set; }
            [DataMember]
            public string team1goals { get; set; }
            [DataMember]
            public string team2 { get; set; }
            [DataMember]
            public string team2goals { get; set; }
        }
      

       public static int getWinnerTotalGoals(string competition, int year)
        {

            var competationurl = "https://jsonmock.hackerrank.com/api/football_competitions?name=" + competition + "&year=" + year;
            string winnername = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(competationurl);
            using(var response = (HttpWebResponse)request.GetResponse())
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string json = reader.ReadToEnd();
                
                using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(json)))
                {
                    // Deserialization from JSON
                    DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(ODataResponse<CompetationRes>));
                    ODataResponse<CompetationRes> bsObj2 = (ODataResponse<CompetationRes>)deserializer.ReadObject(ms);
                    winnername = bsObj2.data[0].winner;
                }
            }
            int total = 0;

            total += getTeamGoals(competition, year, winnername, 1);
            total += getTeamGoals(competition, year, winnername, 2);

            return total;

        }

        public static int getTeamGoals(string competition, int year, string winnername, int whichteam=1)
        {
            string whichTmGoals = "team" + whichteam+ "goals";
            var matchesurl = "https://jsonmock.hackerrank.com/api/football_matches?competation=" + competition + "&team" + whichteam + "=" + winnername + "&year=" + year;
            int totalpages = 0;
            int total = 0;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(matchesurl);
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string json = reader.ReadToEnd();
                using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(json)))
                {
                    // Deserialization from JSON
                    DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(ODataResponse<MatchResonse>));
                    ODataResponse<MatchResonse> bsObj2 = (ODataResponse<MatchResonse>)deserializer.ReadObject(ms);
                    totalpages = bsObj2.total_pages;
                }
            }

            for(int i =1; i<=totalpages; i++)
            {
                HttpWebRequest request1 = (HttpWebRequest)WebRequest.Create("https://jsonmock.hackerrank.com/api/football_matches?competation=" + competition + "&year=" + year + "&team1=" + winnername + "&page=" + i);
                using (var response = (HttpWebResponse)request1.GetResponse())
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string json = reader.ReadToEnd();
                    using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(json)))
                    {
                        // Deserialization from JSON
                        DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(ODataResponse<MatchResonse>));
                        ODataResponse<MatchResonse> bsObj2 = (ODataResponse<MatchResonse>)deserializer.ReadObject(ms);
                        totalpages = bsObj2.total_pages;
                        for (int j = 0; j < bsObj2.data.Length; j++)
                            total += Convert.ToInt32(typeof(MatchResonse).GetProperty(whichTmGoals).GetValue(bsObj2.data[j]));
                    }
                }
            }



            //for (int i = 1; i <= totalpages; i++)
            //{
            //    using (var client = new HttpClient())
            //    {
            //        var result = client.GetAsync(").Result;
            //        var response = result.Content.ReadAsStringAsync().Result;
            //        var json = System.Text.Json.JsonSerializer.Deserialize<ODataResponse<MatchResonse>>(response).data;
                    
            //        for (int j = 0; j < json.Length; j++)
            //            total += Convert.ToInt32(json[0].GetType().GetProperty(whichTmGoals).GetValue(json[j]));
            //    }
            //}
            return total;
        }
    }
}
