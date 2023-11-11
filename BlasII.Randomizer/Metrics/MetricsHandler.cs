using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BlasII.Randomizer.Metrics
{
    public class MetricsHandler
    {
        public async Task SendSettings(RandomizerSettings settings)
        {
            if (!Main.Randomizer.ConfigHandler.GetProperty<bool>("SendMetrics"))
                return;

            string url = $"http://blas2.randomizer.mooo.com/api/settings";
            string json = JsonConvert.SerializeObject(new MetricsData(settings));

            Main.Randomizer.Log($"Attempting to send metrics data...");

            try
            {
                var client = new HttpClient();
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    throw new Exception(responseContent);
                }

                Main.Randomizer.Log("Successfully sent metrics data");
            }
            catch (Exception e)
            {
                Main.Randomizer.LogError("Failed to send metrics data: " + e.Message);
            }
        }
    }
}
