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

        public async Task<HttpResponseMessage> AddAirport(AirportContract airportContract, string addAirportUrl)
        {
            var response = await _httpClient.PostAsync(
                addAirportUrl,
                new StringContent(JsonConvert.SerializeObject(airportContract),
                Encoding.UTF8, "application/json"));

            return response;
        }

        public async Task<HttpResponseMessage> AddPlane(PlaneContract planeContract, string addPlaneUrl)
        {
            var response = await _httpClient.PostAsync(
                addPlaneUrl,
                new StringContent(JsonConvert.SerializeObject(planeContract),
                Encoding.UTF8, "application/json"));

            return response;
        }

        public async Task<HttpResponseMessage> UpdateAirport(AirportContract airportContract, string updateAirportUrl)
        {
            var response = await _httpClient.PutAsync(
                updateAirportUrl,
                new StringContent(JsonConvert.SerializeObject(airportContract),
                Encoding.UTF8, "application/json"));

            return response;
        }

        public async Task<HttpResponseMessage> UpdatePlane(PlaneContract planeContract, string updatePlaneUrl)
        {
            var response = await _httpClient.PutAsync(
                updatePlaneUrl,
                new StringContent(JsonConvert.SerializeObject(planeContract),
                Encoding.UTF8, "application/json"));

            return response;
        }

        public async Task<List<AirportContract>> GetCurrentlyAvailableAirports(string trafficApiGetAirportsUrl)
        {
            var response = await _httpClient.GetAsync(trafficApiGetAirportsUrl);
            var json = await response.Content.ReadAsStringAsync();
            var airports = JsonConvert.DeserializeObject<List<AirportContract>>(json);

            return airports;
        }

        public async Task<AirportContract> GetAirport(string trafficApiGetAirportUrl, string airportName)
        {
            var response = await _httpClient.GetAsync(trafficApiGetAirportUrl + "/" + airportName);
            var json = await response.Content.ReadAsStringAsync();
            var airport = JsonConvert.DeserializeObject<AirportContract>(json);

            return airport;
        }
    }
}
