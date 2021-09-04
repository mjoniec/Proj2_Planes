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

        public async Task KeepTryingAddAirportUntilSuccessful(AirportContract airportContract, string addAirportUrl)
        {
            var response = await _httpClient.PostAsync(
                addAirportUrl,
                new StringContent(JsonConvert.SerializeObject(airportContract),
                Encoding.UTF8, "application/json"));

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return;
            }
            else
            {
                await Task.Delay(5000);

                await KeepTryingAddAirportUntilSuccessful(airportContract, addAirportUrl);
            }
        }

        public async Task AddPlane(PlaneContract planeContract, string addPlaneUrl)
        {
            await _httpClient.PostAsync(
                addPlaneUrl,
                new StringContent(JsonConvert.SerializeObject(planeContract),
                Encoding.UTF8, "application/json"));
        }

        public async Task KeepTryingAddPlaneUntilSuccessful(PlaneContract planeContract, string addPlaneUrl)
        {
            var response = await _httpClient.PostAsync(
                addPlaneUrl,
                new StringContent(JsonConvert.SerializeObject(planeContract),
                Encoding.UTF8, "application/json"));

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return;
            }
            else
            {
                await Task.Delay(5000);

                await KeepTryingAddPlaneUntilSuccessful(planeContract, addPlaneUrl);
            }
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
