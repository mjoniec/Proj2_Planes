using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Model;
using Mqtt.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PlaneService
{
    public class PlaneService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private readonly IOptions<AirportConfig> _config;
        private readonly IMqttClientSubscriber _mqttClientSubscriber;

        public PlaneService(
            ILogger<AirportConfig> logger,
            IOptions<AirportConfig> config,
            IMqttClientSubscriber mqttClientSubscriber)
        {
            _logger = logger;
            _config = config;
            _mqttClientSubscriber = mqttClientSubscriber;

            _mqttClientSubscriber.Start();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting plane service: " + _config.Value.Name);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping plane service " + _config.Value.Name);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _logger.LogInformation("Disposing plane service");
        }
    }
}
