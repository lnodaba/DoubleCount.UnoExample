using DCx.web.BlazorWasm.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DCx.WpfClient.ApiHelper
{
    public static class TestApiClient
    {
        private static string baseUrl = "https://localhost:44324/WeatherForecast";
        public static async Task<List<string>> GetData(string accessToken) 
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var responseMessage = await client.GetAsync(baseUrl);
            var responseJson    = await responseMessage.Content.ReadAsStringAsync();

          //using JsonTextReader sr = new JsonTextReader(new StringReader(responseJson));
          //var records = new JsonSerializer().Deserialize<List<WeatherForecast>>(sr);

            var records         = JsonSerializer.Deserialize<List<WeatherForecast>>(responseJson);
            return records.Select(x => $"{x.Date} - {x.Summary} - {x.TemperatureC} - {x.TemperatureF}")
                .ToList();
        }
    }
}
