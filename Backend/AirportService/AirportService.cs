using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Model;
using Mqtt;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AirportService
{
    public class AirportService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private readonly IOptions<AirportConfig> _config;
        private readonly IMqttClient _mqttClient;

        public AirportService(
            ILogger<AirportConfig> logger,
            IOptions<AirportConfig> config,
            IMqttClient mqttClient)
        {
            _logger = logger;
            _config = config;
            _mqttClient = mqttClient;

            //mqttClient.RaiseMessageReceivedEvent += RequestReceivedHandler;

            //_mqttClient.Start();
        }

        public void RequestReceivedHandler(object sender, MessageEventArgs e)
        {

        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting airport service: " + _config.Value.Name);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping external gold data api client service.");

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _logger.LogInformation("Disposing....");
        }
    }
}
