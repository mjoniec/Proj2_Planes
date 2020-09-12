using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Model;
using Mqtt.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AirportService
{
    public class AirportService : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly IOptions<AirportConfig> _config;
        private readonly IMqttClientPublisher _mqttClientPublisher;

        public AirportService(
            ILogger<AirportConfig> logger,
            IOptions<AirportConfig> config,
            IMqttClientPublisher mqttClientPublisher)
        {
            _logger = logger;
            _config = config;
            _mqttClientPublisher = mqttClientPublisher;

            _mqttClientPublisher.Start();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug($"AirportService is starting.");

            stoppingToken.Register(() =>
                _logger.LogDebug($" AirportService background task is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"Weather condition changed");

                ChangeWeather();

                await Task.Delay(4000, stoppingToken);
            }
        }

        private void ChangeWeather()
        {
            _mqttClientPublisher.PublishAsync("Wheather status change for airport: " + "airport test 1");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting airport service: " + _config.Value.Name);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping airport service " + _config.Value.Name);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _logger.LogInformation("Disposing airport service");
        }
    }
}
