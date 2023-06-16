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

    public async Task CreateCampaign(string adAccountId, string name, string objective, string status, List<string> specialAdCategories)
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
            Console.WriteLine("Campaign created successfully");
        }
        else
        {
            Console.WriteLine($"Failed to create campaign: {responseString}");
        }
    }
}
