using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Server;

namespace MQTTDaemon
{

    public class MQTTService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private readonly IOptions<MQTTConfig> _config;
        private IMqttServer _mqttServer;
        public MQTTService(ILogger<MQTTService> logger, IOptions<MQTTConfig> config)
        {
            _logger = logger;
            _config = config;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting MQTT Daemon on port " + _config.Value.Port);

            //Building the config
            var optionsBuilder = new MqttServerOptionsBuilder()
                .WithConnectionBacklog(1000)
                .WithDefaultEndpointPort(_config.Value.Port);


            //Getting an MQTT Instance
            _mqttServer = new MqttFactory().CreateMqttServer();

            //Wiring up all the events...

            _mqttServer.ClientSubscribedTopic += _mqttServer_ClientSubscribedTopic;
            _mqttServer.ClientUnsubscribedTopic += _mqttServer_ClientUnsubscribedTopic;
            _mqttServer.ClientConnected += _mqttServer_ClientConnected;
            _mqttServer.ClientDisconnected += _mqttServer_ClientDisconnected;
            _mqttServer.ApplicationMessageReceived += _mqttServer_ApplicationMessageReceived;

            //Now, start the server -- Notice this is resturning the MQTT Server's StartAsync, which is a task.
            return _mqttServer.StartAsync(optionsBuilder.Build());
        }

        private void _mqttServer_ApplicationMessageReceived(object sender, MqttApplicationMessageReceivedEventArgs e)
        {
            _logger.LogInformation(e.ClientId + " published message to topic " + e.ApplicationMessage.Topic);
        }

        private void _mqttServer_ClientDisconnected(object sender, MqttClientDisconnectedEventArgs e)
        {
            _logger.LogInformation(e.ClientId + " Disonnected.");
        }

        private void _mqttServer_ClientConnected(object sender, MqttClientConnectedEventArgs e)
        {
            _logger.LogInformation(e.ClientId + " Connected.");
        }

        private void _mqttServer_ClientUnsubscribedTopic(object sender, MqttClientUnsubscribedTopicEventArgs e)
        {
            _logger.LogInformation(e.ClientId + " unsubscribed to " + e.TopicFilter);
        }

        private void _mqttServer_ClientSubscribedTopic(object sender, MqttClientSubscribedTopicEventArgs e)
        {
            _logger.LogInformation(e.ClientId + " subscribed to " + e.TopicFilter);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping MQTT Daemon.");
            return _mqttServer.StopAsync();
        }

        public void Dispose()
        {
            _logger.LogInformation("Disposing....");

        }
    }
}
