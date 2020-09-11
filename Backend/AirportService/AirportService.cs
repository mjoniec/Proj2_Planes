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
    public class AirportService : IHostedService, IDisposable
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

            //testing
            _mqttClientPublisher.PublishAsync("Initialize message for airport service: " + _config.Value.Name);
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
