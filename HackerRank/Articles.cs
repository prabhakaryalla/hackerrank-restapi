using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace HackerRank
{
    public static class Articles
    {
        public static List<string> getArticleTitles(string author)
        {
            HttpClient httpClient = new HttpClient();
            int totalPages = 1, pageNumber = 1;
            string Url = String.Empty;
            List<string> Articles = new List<string>();
            while (pageNumber <= totalPages)
            {
                Url = $"https://jsonmock.hackerrank.com/api/articles?author={author}&page={pageNumber}";
                HttpResponseMessage responseMessage = httpClient.GetAsync(Url).Result;
                if (responseMessage.IsSuccessStatusCode)
                {
                    var Result = responseMessage.Content.ReadAsStringAsync().Result;
                    var jsonData = JObject.Parse(Result);
                    if (pageNumber == 1)
                    {
                        totalPages = (int)jsonData["total_pages"];
                    }
                    var records = jsonData["data"];
                    foreach (var item in records)
                    {
                        var title = item["title"].ToString();
                        var story_title = item["story_title"].ToString();
                        if (title != "")
                        {
                            Articles.Add(title);
                        }
                        else if (title == "" && story_title != "")
                        {
                            Articles.Add(story_title);
                        }
                    }
                }
                pageNumber++;
            }
            return Articles;
        }
    }
}
