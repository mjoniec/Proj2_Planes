using Contracts;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HttpUtils
{
    public class TrafficInfoHttpClient
    {
        private readonly HttpClient _httpClient;

        public TrafficInfoHttpClient()
        {
            _httpClient = new HttpClient();
        }

        public async Task PostPlaneInfo(PlaneContract planeContract, string airTrafficApiUpdatePlaneInfoUrl)
        {
            await _httpClient.PostAsync(
                airTrafficApiUpdatePlaneInfoUrl,
                new StringContent(JsonConvert.SerializeObject(planeContract),
                Encoding.UTF8, "application/json"));
        }

        public async Task<List<AirportContract>> GetCurrentlyAvailableAirports(string airTrafficApiGetAirportsUrl)
        {
            var response = await _httpClient.GetAsync(airTrafficApiGetAirportsUrl);
            var json = await response.Content.ReadAsStringAsync();
            var airports = JsonConvert.DeserializeObject<List<AirportContract>>(json);

            return airports;
        }
    }
}
