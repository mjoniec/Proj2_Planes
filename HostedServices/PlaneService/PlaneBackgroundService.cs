using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MqttUtils;
using PlaneService.Domain;
using System.Threading;
using System.Threading.Tasks;
using Utils;

namespace PlaneService
{
    public class PlaneBackgroundService : BackgroundService
    {
        private readonly Plane _plane;
        private readonly TrafficInfoHttpClient _trafficInfoHttpClient;
        private readonly string TrafficInfoApiUpdatePlaneUrl;
        private readonly string TrafficInfoApiUpdateGetAirportsUrl;
        private readonly MqttClientSubscriber _mqttClientSubscriber;

        public PlaneBackgroundService(
            IConfiguration configuration, 
            IHostEnvironment hostEnvironment,
            MqttClientSubscriber mqttClientSubscriber)
        {
            //required to install nuget: Microsoft.Extensions.Configuration.Binder
            var name = HostServiceNameSelector.AssignName("Plane", hostEnvironment.EnvironmentName, configuration.GetValue<string>("name"));

            _plane = new Plane(name);
            _trafficInfoHttpClient = new TrafficInfoHttpClient();
            
            TrafficInfoApiUpdatePlaneUrl = configuration.GetValue<string>(nameof(TrafficInfoApiUpdatePlaneUrl));
            TrafficInfoApiUpdateGetAirportsUrl = configuration.GetValue<string>(nameof(TrafficInfoApiUpdateGetAirportsUrl));

            //mqtt related
            _mqttClientSubscriber = mqttClientSubscriber;
            _mqttClientSubscriber.RaiseMessageReceivedEvent += MqttMessageReceivedHandler;
            _ = _mqttClientSubscriber.Start();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _plane.StartPlane(await _trafficInfoHttpClient.GetCurrentlyAvailableAirports(TrafficInfoApiUpdateGetAirportsUrl));
            
            await _mqttClientSubscriber.SubscribeToTopic(_plane.PlaneContract.DestinationAirportName);

            while (!stoppingToken.IsCancellationRequested)
            {
                //plane management logic should go to a separate domain lifecycle manager, exported as a tick for a loop holder object

                _plane.UpdatePlane();

                if (_plane.PlaneReachedItsDestination)
                {
                    await SelectNewDestinationAirport();
                }

                await _trafficInfoHttpClient.PostPlaneInfo(_plane.PlaneContract, TrafficInfoApiUpdatePlaneUrl);
                await Task.Delay(1000, stoppingToken);
            }
        }

        /// <summary>
        /// Upon receiving the event changes plane's destination airport
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void MqttMessageReceivedHandler(object sender, MessageEventArgs e)
        {
            await SelectNewDestinationAirport();
        }

        private async Task SelectNewDestinationAirport()
        {
            var airports = await _trafficInfoHttpClient.GetCurrentlyAvailableAirports(TrafficInfoApiUpdateGetAirportsUrl);

            _plane.SelectNewDestinationAirport(airports);

            await _mqttClientSubscriber.SubscribeToTopic(_plane.PlaneContract.DestinationAirportName);
        }
    }
}
