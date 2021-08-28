using AirportService.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MqttUtils;
using System;
using System.Threading;
using System.Threading.Tasks;
using Utils;

namespace AirportService
{
    public class AirportBackgroundService : BackgroundService
    {
        private readonly ILogger<AirportBackgroundService> _logger;
        private readonly string TrafficInfoApiUpdateAirportUrl;
        private readonly Airport _airport;
        private readonly TrafficInfoHttpClient _trafficInfoHttpClient;
        private readonly MqttClientPublisher _mqttClientPublisher;

        //this logic belongs in domain object lifecycle manager, not in the contract or domain object itself. 
        //If bad weather event happened we want to send mqtt notification once.
        private bool _badWeatherInfoSent;

        public AirportBackgroundService(
            ILogger<AirportBackgroundService> logger,
            Airport airport,
            IConfiguration configuration, 
            MqttClientPublisher mqttClientPublisher)
        {
            TrafficInfoApiUpdateAirportUrl = configuration.GetValue<string>(nameof(TrafficInfoApiUpdateAirportUrl));
            _logger = logger;
            _airport = airport;
            _trafficInfoHttpClient = new TrafficInfoHttpClient();
            _mqttClientPublisher = mqttClientPublisher;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _mqttClientPublisher.Start();

            //airport management logic should go to a separate domain lifecycle manager, exported as a tick for a loop holder object
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Execute loop at " + DateTime.Now.ToString("G"));

                await _airport.UpdateAirport();

                //TODO - do not await with further execution untill response received ? add fallback or sth ?
                await _trafficInfoHttpClient.PostAirportInfo(_airport.AirportContract, TrafficInfoApiUpdateAirportUrl);

                //TODO these business conditions ll have to get unit tested
                if (_airport.AirportContract.IsGoodWeather)
                {
                    _logger.LogInformation("good weather");
                    _badWeatherInfoSent = false;//set up ready for next bad weather publish
                }
                else
                {
                    _logger.LogInformation("bad weather");

                    if (!_badWeatherInfoSent)
                    {
                        _logger.LogInformation("sending bad weather info to mqtt topic:" + _airport.AirportContract.Name);

                        _badWeatherInfoSent = true;//prevents from messages beeing continually sent while the weather is bad

                        await _mqttClientPublisher.PublishAsync("Wheather status change for airport: " + 
                            _airport.AirportContract.Name, _airport.AirportContract.Name);
                    }
                }

                await Task.Delay(4000, stoppingToken);
            }
        }
    }
}
