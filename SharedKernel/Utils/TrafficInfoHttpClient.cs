using Contracts;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class TrafficInfoHttpClient
    {
        private readonly HttpClient _httpClient;

        public TrafficInfoHttpClient()
        {
            _httpClient = new HttpClient();
        }

        public async Task PostAirportInfo(AirportContract airportContract, string airTrafficApiUpdateAirportInfoUrl)
        {
            await _httpClient.PostAsync(
                    airTrafficApiUpdateAirportInfoUrl,
                    new StringContent(JsonConvert.SerializeObject(airportContract),
                    Encoding.UTF8, "application/json"));
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

        public async Task<AirportContract> GetAirport(string airTrafficApiGetAirportUrl, string airportName)
        {
            var response = await _httpClient.GetAsync(airTrafficApiGetAirportUrl + "/" + airportName);
            var json = await response.Content.ReadAsStringAsync();
            var airport = JsonConvert.DeserializeObject<AirportContract>(json);

            return airport;
        }
    }
}
