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

        public async Task AddAirport(AirportContract airportContract, string addAirportUrl)
        {
            await _httpClient.PostAsync(
                    addAirportUrl,
                    new StringContent(JsonConvert.SerializeObject(airportContract),
                    Encoding.UTF8, "application/json"));
        }

        public async Task AddPlane(PlaneContract planeContract, string addPlaneUrl)
        {
            await _httpClient.PostAsync(
                addPlaneUrl,
                new StringContent(JsonConvert.SerializeObject(planeContract),
                Encoding.UTF8, "application/json"));
        }

        public async Task UpdateAirport(AirportContract airportContract, string updateAirportUrl)
        {
            await _httpClient.PutAsync(
                    updateAirportUrl,
                    new StringContent(JsonConvert.SerializeObject(airportContract),
                    Encoding.UTF8, "application/json"));
        }

        public async Task UpdatePlane(PlaneContract planeContract, string updatePlaneUrl)
        {
            await _httpClient.PutAsync(
                updatePlaneUrl,
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
