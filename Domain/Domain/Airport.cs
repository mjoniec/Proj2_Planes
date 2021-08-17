using AirTrafficInfoContracts;
using Microsoft.Extensions.Configuration;//TODO get rid of these http specific clients, export to shared kernel expose through interface
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Domain
{
    public class Airport
    {
        private readonly string AirTrafficApiUpdateAirportInfoUrl;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly HttpClient _httpClient;
        private readonly AirportContract _airportContract;

        public Airport(IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            //required to install nuget: Microsoft.Extensions.Configuration.Binder
            var name = configuration.GetValue<string>("name");
            var color = configuration.GetValue<string>("color");
            var latitude = configuration.GetValue<string>("latitude");
            var longitude = configuration.GetValue<string>("longitude");

            AirTrafficApiUpdateAirportInfoUrl = configuration.GetValue<string>(nameof(AirTrafficApiUpdateAirportInfoUrl));

            _hostEnvironment = hostEnvironment;
            _httpClient = new HttpClient();
            _airportContract = new AirportContract
            {
                Name = AssignName(name),
                Color = string.IsNullOrEmpty(color) ? "#" + new Random().Next(100000, 999999).ToString() : color,
                Latitude = string.IsNullOrEmpty(latitude) ? new Random().Next(-150, 170) : double.Parse(latitude, CultureInfo.InvariantCulture),
                Longitude = string.IsNullOrEmpty(longitude) ? new Random().Next(-55, 70) : double.Parse(longitude, CultureInfo.InvariantCulture)
            };
        }

        public async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _httpClient.PostAsync(
                    AirTrafficApiUpdateAirportInfoUrl,
                    new StringContent(JsonConvert.SerializeObject(_airportContract),
                    Encoding.UTF8, "application/json"));

                await Task.Delay(1100, stoppingToken);
            }
        }

        /// <summary>
        /// we do not want the name on production to have anything other than city name
        /// we also want to see more info easily on non production environments
        /// </summary>
        /// <param name="name"></param>
        private string AssignName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                if (_hostEnvironment.EnvironmentName == "Development")//manual on premises launch from visual studio
                {
                    name = "Airport_" + _hostEnvironment.EnvironmentName + "_" + new Random().Next(1001, 9999).ToString();
                }
                else if (_hostEnvironment.EnvironmentName == "Docker")
                {
                    name = "Error - AirportNameShouldHaveBeenGivenFor_" + _hostEnvironment.EnvironmentName + "_Environment_" + new Random().Next(1001, 9999).ToString();
                }
                else
                {
                    name = "Warning - Unpredicted Environment - Airport_" + _hostEnvironment.EnvironmentName + "_" + new Random().Next(1001, 9999).ToString();
                }
            }
            else
            {
                if (_hostEnvironment.EnvironmentName == "Development")//on premises launch from ps script
                {
                    name += "_" + _hostEnvironment.EnvironmentName;
                }
                else if (_hostEnvironment.EnvironmentName == "Docker")
                {
                    //production name - expected to be displayed as given from docker compose
                }
                else
                {
                    name += "Warning - Unpredicted Environment - Airport_" + _hostEnvironment.EnvironmentName + "_" + new Random().Next(1001, 9999).ToString();
                }
            }

            return name;
        }
    }
}
