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

namespace HackerRank
{
   static class RestAPIMatchGoals
    {
        public class ODataResponse<T>
        {
            public int page { get; set; }
            public int per_page { get; set; }
            public int total_pages { get; set; }
            public T[] data { get; set; }
        }

        public class CompetationRes
        {
            public string winner { get; set; }
        }

        public class MatchResonse
        {
            public string team1 { get; set; }
            public string team1goals { get; set; }
            public string team2 { get; set; }
            public string team2goals { get; set; }
        }
      

        public static int getWinnerTotalGoals(string competition, int year)
        {
            var competationurl = "https://jsonmock.hackerrank.com/api/football_competitions?name=" + competition + "&year=" + year;
            string winnername = "";
            using (var client = new HttpClient())
            {
                var result = client.GetAsync(competationurl).Result;
                var response = result.Content.ReadAsStringAsync().Result;
                var json = System.Text.Json.JsonSerializer.Deserialize<ODataResponse<CompetationRes>>(response);
                winnername = json.data[0].winner;
            }

            int total = 0;
            total += getTeamGoals(competition, year, winnername, 1);
            total += getTeamGoals(competition, year, winnername, 2);

            return total;

        }

        public static int getTeamGoals(string competition, int year, string winnername, int whichteam=1)
        {
            string whichTmGoals = "team" + whichteam+ "goals";
            var matchesurl = "https://jsonmock.hackerrank.com/api/football_matches?competition=" + competition + "&year=" + year + "&team" + whichteam + "=" + winnername;
            int totalpages = 0;

            using (var client = new HttpClient())
            {
                var result = client.GetAsync(matchesurl).Result;
                var response = result.Content.ReadAsStringAsync().Result;
                var json = System.Text.Json.JsonSerializer.Deserialize<ODataResponse<MatchResonse>>(response);
                totalpages = json.total_pages;
            }

            int total = 0;
            for (int i = 1; i <= totalpages; i++)
            {
                using (var client = new HttpClient())
                {
                    var result = client.GetAsync("https://jsonmock.hackerrank.com/api/football_matches?competition=" + competition + "&year=" + year + "&team" + whichteam + "=" + winnername + "&page=" + i).Result;
                    var response = result.Content.ReadAsStringAsync().Result;
                    var json = System.Text.Json.JsonSerializer.Deserialize<ODataResponse<MatchResonse>>(response).data;
                    
                    for (int j = 0; j < json.Length; j++)
                        total += Convert.ToInt32(json[0].GetType().GetProperty(whichTmGoals).GetValue(json[j]));
                }
            }
            return total;
        }
    }
}
