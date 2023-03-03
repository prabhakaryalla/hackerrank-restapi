using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace HackerRank
{
    public static class Restaurant
    {
        public static string bestRestaurant(string city, int cost)
        {
            HttpClient httpClient = new HttpClient();
            int totalPages = 1, pageNumber = 1;
            string Url = String.Empty;
            //string bestRestaurant = string.Empty;
            //decimal bestAvgRating = 0;
            Dictionary<string, decimal> resturants = new Dictionary<string, decimal>();
            while (pageNumber <= totalPages)
            {
                Url = $"https://jsonmock.hackerrank.com/api/food_outlets?page={pageNumber}";
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
                        var cityName = item["city"].ToString();
                        var estimated_cost = Convert.ToInt32(item["estimated_cost"]);


                        if (city == cityName && estimated_cost <= cost)
                        {
                            decimal avgRating = 0;
                            var userRating = item["user_rating"];
                            if (userRating != null)
                            {
                                avgRating = Convert.ToDecimal(userRating["average_rating"]);
                            }
                            if(!resturants.ContainsKey(item["name"].ToString()))
                            {
                                resturants.Add(item["name"].ToString(), avgRating);
                            }
                            

                            //if (avgRating > bestAvgRating)
                            //{
                            //    bestAvgRating = avgRating;
                            //    bestRestaurant = item["name"].ToString();
                            //}

                        }
                    }
                }
                pageNumber++;
            }
            if(resturants.Count() > 0)
            {
                var maxRating = resturants.Values.Max();
                var bestrest = resturants.Where(rest => rest.Value == maxRating).OrderBy(b => b.Key).FirstOrDefault().Key;
                return bestrest;
            }
            return null;
           
        }

    }
}
