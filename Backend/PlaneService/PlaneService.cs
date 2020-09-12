using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Model;
using Mqtt;
using Mqtt.Interfaces;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace PlaneService
{
    public class PlaneService : BackgroundService
    {
        private readonly ILogger<PlaneService> _logger;
        private readonly IOptions<AirportConfig> _config;
        private readonly IMqttClientSubscriber _mqttClientSubscriber;
        private readonly HttpClient _httpClient = new HttpClient();

        private Plane _plane = new Plane
        {
            Name = "plane test 1 ",
            Speed = 1.0
        };

        public PlaneService(
            ILogger<PlaneService> logger,
            IOptions<AirportConfig> config,
            IMqttClientSubscriber mqttClientSubscriber)
        {
            _logger = logger;
            _config = config;
            _mqttClientSubscriber = mqttClientSubscriber;
            _mqttClientSubscriber.RaiseMessageReceivedEvent += RequestReceivedHandler;

            ChangeDirectionPlane();
            _mqttClientSubscriber.Start();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug($"PlaneService is starting.");

            stoppingToken.Register(() =>
                _logger.LogDebug($" PlaneService is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"Plane " + _plane.Name + " position: " +_plane.PositionLatitude + " " + _plane.PositionLongitude );

                MovePlane();
                await NotifyAirTrafficApi();

                await Task.Delay(1000, stoppingToken);
            }
        }

        private async Task NotifyAirTrafficApi()
        {
            //TODO: take URL from appsettings or inject through docker somehow
            await _httpClient.PostAsync(
                $"https://localhost:44389/api/airtrafficinfo/{_plane.Name}/{_plane.PositionLatitude}/{_plane.PositionLongitude}",
                new StringContent(""));
        }

        private void MovePlane()
        {
            var distanceCovered = DateTime.Now.Subtract(_plane.DepartureTime).TotalSeconds * _plane.Speed;

            _plane.PositionLatitude = _plane.DepartureLatitude + distanceCovered;
            _plane.PositionLongitude = _plane.DepartureLongitude + distanceCovered;
        }

        private void ChangeDirectionPlane()
        {
            _plane.DepartureLatitude = 0.0;
            _plane.DepartureLongitude = 0.0;
            _plane.DestinationLatitude = 1000.0;
            _plane.DestinationLongitude = 1000.0;
            _plane.DepartureTime = DateTime.Now.AddSeconds(-3.0);
        }

        public void RequestReceivedHandler(object sender, MessageEventArgs e)
        {
            _logger.LogInformation("Plane service received message: " + e.Message);

            //some if for to turn plane or not
            ChangeDirectionPlane();
        }

        //public override async Task StartAsync(CancellationToken cancellationToken)
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting plane service: " + _config.Value.Name);

            return Task.CompletedTask;
        }

        //public override async Task StopAsync(CancellationToken cancellationToken)
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping plane service " + _config.Value.Name);

            return Task.CompletedTask;
        }

        //public override void Dispose()
        public void Dispose()
        {
            _logger.LogInformation("Disposing plane service");
        }
    }
}
