using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HackerRank
{
    public class ODataResponse<T>
    {
        public int page { get; set; }
        public int per_page { get; set; }
        public int total_pages { get; set; }
        public T[] data { get; set; }
    }

    public class MatchResonse
    {
        public string team1 { get; set; }
        public string team1goals { get; set; }
        public string team2 { get; set; }
        public string team2goals { get; set; }
    }
    public static class MatchesDrawn
    {
        public static int getNumDraws(int year)
        {
            //int res = 0;
            var matchesurl = "https://jsonmock.hackerrank.com/api/football_matches?year=" + year;
           ConcurrentBag<int> res = new ConcurrentBag<int>();

            int totalpages = 0;
            using (var client1 = new HttpClient())
            {
                var result = client1.GetAsync(matchesurl).Result;
                var response = result.Content.ReadAsStringAsync().Result;
                var json = System.Text.Json.JsonSerializer.Deserialize<ODataResponse<MatchResonse>>(response);
                totalpages = json.total_pages;
            }

            using var client = new HttpClient();
            Parallel.For(1, totalpages +1,   new ParallelOptions() { MaxDegreeOfParallelism = 12 }, i =>
           {
               var pagematchesurl = "https://jsonmock.hackerrank.com/api/football_matches?year=" + year + "&page=" + i;
               var result = client.GetAsync(pagematchesurl).Result;
               var response = result.Content.ReadAsStringAsync().Result;
               var json = System.Text.Json.JsonSerializer.Deserialize<ODataResponse<MatchResonse>>(response);
               var count = (from s in json.data where s.team1goals == s.team2goals select s).Count();
               res.Add(count);
           });

            return res.Sum();

            //var loadPosts = new List<Task<string>>();
            //foreach (var post in list)
            //{
            //    var response = await client.GetAsync("posts/" + post);

            //    var contents = response.Content.ReadAsStringAsync();
            //    loadPosts.Add(contents);
            //    Console.WriteLine(contents.Result);
            //}

            //await Task.WhenAll(loadPosts);



            //for (int i = 1; i <= totalpages; i++)
            //{
            //    var pagematchesurl = "https://jsonmock.hackerrank.com/api/football_matches?year=" + year + "&page=" + i;
            //    using (var client = new HttpClient())
            //    {
            //        var result = client.GetAsync(pagematchesurl).Result;
            //        var response = result.Content.ReadAsStringAsync().Result;
            //        var json = System.Text.Json.JsonSerializer.Deserialize<ODataResponse<MatchResonse>>(response);
            //        res += (from s in json.data where s.team1goals == s.team2goals select s).Count();
            //    }
            //}

            //return res;
        }


    }
}
