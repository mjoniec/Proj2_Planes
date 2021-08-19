using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PlaneHostedService
{
    public class PlaneBackgroundService : BackgroundService
    {
        private readonly Plane _plane;

        private readonly HttpClient _httpClient;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly string AirTrafficApiUpdatePlaneInfoUrl;
        private readonly string AirTrafficApiGetAirportsUrl;

        public PlaneBackgroundService(IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            //required to install nuget: Microsoft.Extensions.Configuration.Binder
            var name = AssignName(configuration.GetValue<string>("name"));

            _plane = new Plane(name);
            _httpClient = new HttpClient();
            _hostEnvironment = hostEnvironment;
            AirTrafficApiUpdatePlaneInfoUrl = configuration.GetValue<string>(nameof(AirTrafficApiUpdatePlaneInfoUrl));
            AirTrafficApiGetAirportsUrl = configuration.GetValue<string>(nameof(AirTrafficApiGetAirportsUrl));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //await _plane.ExecuteAsync(stoppingToken);

            await _plane.StartPlane();

            while (!stoppingToken.IsCancellationRequested)
            {
                await _plane.UpdatePlane();

                await _httpClient.PostAsync(
                    AirTrafficApiUpdatePlaneInfoUrl,
                    new StringContent(JsonConvert.SerializeObject(_planeContract),
                    Encoding.UTF8, "application/json"));

                await Task.Delay(600, stoppingToken);
            }
        }

        /// <summary>
        /// we do not want the name on production to have anything other than pilot name 
        /// we also want to see more info easily on non production environments
        /// </summary>
        /// <param name="name"></param>
        private string AssignName(string name) //TODO this should go to hosted services 
        {
            if (string.IsNullOrEmpty(name))
            {
                if (_hostEnvironment.EnvironmentName == "Development")//manual on premises launch from visual studio
                {
                    name = "Plane_" + _hostEnvironment.EnvironmentName + "_" + new Random().Next(1001, 9999).ToString();
                }
                else if (_hostEnvironment.EnvironmentName == "Docker")
                {
                    name = "Error - PlaneNameShouldHaveBeenGivenFor_" + _hostEnvironment.EnvironmentName + "_Environment_" + new Random().Next(1001, 9999).ToString();
                }
                else
                {
                    name = "Warning - Unpredicted Environment - Plane_" + _hostEnvironment.EnvironmentName + "_" + new Random().Next(1001, 9999).ToString();
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
                    name += "Warning - Unpredicted Environment - Plane_" + _hostEnvironment.EnvironmentName + "_" + new Random().Next(1001, 9999).ToString();
                }
            }

            return name;
        }
    }
}
