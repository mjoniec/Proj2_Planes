using AirportService.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MqttUtils;
using System.Threading;
using System.Threading.Tasks;
using Utils;

namespace AirportService
{
    public class AirportBackgroundService : BackgroundService
    {
        private readonly string TrafficInfoApiUpdateAirportUrl;
        private readonly Airport _airport;
        private readonly TrafficInfoHttpClient _trafficInfoHttpClient;
        private readonly MqttClientPublisher _mqttClientPublisher;
        private readonly IOptions<MqttConfig> _mqttConfig;

        //this logic belongs in domain object lifecycle manager, not in the contract or domain object itself. 
        //If bad weather event happened we want to send mqtt notification once.
        private bool _badWeatherInfoSent;

        public AirportBackgroundService(
            IConfiguration configuration, 
            IHostEnvironment hostEnvironment,
            MqttClientPublisher mqttClientPublisher,
            IOptions<MqttConfig> mqttConfig)
        {
            //required to install nuget: Microsoft.Extensions.Configuration.Binder
            var name = HostServiceNameSelector.AssignName("Airport", hostEnvironment.EnvironmentName, configuration.GetValue<string>("name"));
            var color = configuration.GetValue<string>("color");
            var latitude = configuration.GetValue<string>("latitude");
            var longitude = configuration.GetValue<string>("longitude");

            TrafficInfoApiUpdateAirportUrl = configuration.GetValue<string>(nameof(TrafficInfoApiUpdateAirportUrl));
            _airport = new Airport(name, color, latitude, longitude);
            _trafficInfoHttpClient = new TrafficInfoHttpClient();
            _mqttClientPublisher = mqttClientPublisher;
            _mqttConfig = mqttConfig;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _mqttClientPublisher.Start();

            while (!stoppingToken.IsCancellationRequested)
            {
                await _airport.UpdateAirport();

                //TODO - do not await with further execution untill response received
                //??? add fallback or sth ?
                await _trafficInfoHttpClient.PostAirportInfo(_airport.AirportContract, TrafficInfoApiUpdateAirportUrl);

                //TODO these business conditions ll have to get unit tested at some point
                if (_airport.AirportContract.IsGoodWeather)
                {
                    _badWeatherInfoSent = false;//set up ready for next bad weather publish
                }
                else
                {
                    if (!_badWeatherInfoSent)
                    {
                        _badWeatherInfoSent = true;//prevents from messages beeing continually sent while the weather is bad

                        await _mqttClientPublisher.PublishAsync("Wheather status change for airport: " + _airport.AirportContract.Name);
                    }
                }

                await Task.Delay(5100, stoppingToken);
            }
        }
    }
}
