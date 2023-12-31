﻿using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DotNetEnv;

namespace FacebookDotnet
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Env.Load();
            var ads_account_id = Environment.GetEnvironmentVariable("ADS_ACCOUNT_ID");
            var user_access_key = Environment.GetEnvironmentVariable("USER_ACCESS_KEY");

            if (user_access_key is null || ads_account_id is null)
            {
                Console.WriteLine("Access token or Ad account ID is missing");
                return;
            }

            var api = new FacebookAdsAPI(user_access_key);
            string responseString = await api.CreateCampaign(ads_account_id, "Revx Campaign 2", "OUTCOME_LEADS", "PAUSED", new List<string> { "NONE" });
            
            if (!string.IsNullOrEmpty(responseString))
            {
                var jsonResponse = JsonConvert.DeserializeObject<dynamic>(responseString);
                var id = jsonResponse["id"].ToString();
                
                var targeting = new Dictionary<string, object>
                {
                    { "geo_locations", new Dictionary<string, List<string>> { { "countries", new List<string> { "ZA" } } } }
                };

                await api.CreateAdSet(ads_account_id, id, "My Ad Set", "REACH", "IMPRESSIONS", 2, 10000, "PAUSED", targeting);
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}