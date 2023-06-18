using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

public class FacebookAdsAPI
{
    private readonly HttpClient client;

    public FacebookAdsAPI(string accessToken)
    {
        client = new HttpClient
        {
            BaseAddress = new Uri("https://graph.facebook.com/v17.0/")
        };
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
    }

    public async Task<string> CreateCampaign(string adAccountId, string name, string objective, string status, List<string> specialAdCategories)
    {
        var values = new Dictionary<string, string>
        {
            { "name", name },
            { "objective", objective },
            { "status", status },
            { "special_ad_categories", JsonConvert.SerializeObject(specialAdCategories)}
        };
        var content = new StringContent(JsonConvert.SerializeObject(values), Encoding.UTF8, "application/json");

        var response = await client.PostAsync($"act_{adAccountId}/campaigns", content);

        var responseString = await response.Content.ReadAsStringAsync();

        if(response.IsSuccessStatusCode)
        {   
            Console.WriteLine("Campaign created successfully: " + responseString);
            return responseString;
        }
        else
        {
            Console.WriteLine($"Failed to create campaign: {responseString}");
            return responseString;
        }
    }

    public async Task CreateAdSet(string adAccountId, string campaignId, string name, string optimizationGoal, string billingEvent, int bidAmount, int dailyBudget, string status, object targeting)
    {
        var values = new Dictionary<string, object>
        {
            { "name", name },
            { "optimization_goal", optimizationGoal },
            { "billing_event", billingEvent },
            { "bid_amount", bidAmount },
            { "daily_budget", dailyBudget },
            { "campaign_id", campaignId },
            { "status", status },
            { "targeting", targeting }
        };
        var content = new StringContent(JsonConvert.SerializeObject(values), Encoding.UTF8, "application/json");

        var response = await client.PostAsync($"act_{adAccountId}/adsets", content);

        var responseString = await response.Content.ReadAsStringAsync();
        
        if(response.IsSuccessStatusCode)
        {
            Console.WriteLine("Ad set created successfully: " + responseString);
            // continue here
        }
        else
        {
            Console.WriteLine($"Failed to create ad set: {responseString}");
        }
    }
}
