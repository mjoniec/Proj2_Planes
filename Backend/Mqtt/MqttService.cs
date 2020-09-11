using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client.Receiving;
using MQTTnet.Server;

namespace Mqtt
{
    public class MqttService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private readonly IOptions<MqttConfig> _config;
        private IMqttServer _mqttServer;
        public MqttService(ILogger<MqttService> logger, IOptions<MqttConfig> config)
        {
            _logger = logger;
            _config = config;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting MQTT Service on port " + _config.Value.Port);

            //Building the config
            var optionsBuilder = new MqttServerOptionsBuilder()
                .WithConnectionBacklog(1000)
                .WithDefaultEndpointPort(_config.Value.Port);

            //Getting an MQTT Instance
            _mqttServer = new MqttFactory().CreateMqttServer();

            //Wiring up all the events and handlers
            _mqttServer.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandler(_logger);
            _mqttServer.ClientConnectedHandler = new MqttServerClientConnectedHandler(_logger);
            _mqttServer.ClientDisconnectedHandler = new MqttServerClientDisconnectedHandler(_logger);
            _mqttServer.ClientSubscribedTopicHandler = new MqttServerClientSubscribedTopicHandler(_logger);
            _mqttServer.ClientUnsubscribedTopicHandler = new MqttServerClientUnsubscribedTopicHandler(_logger);

            //Now, start the server -- Notice this is resturning the MQTT Server's StartAsync, which is a task.
            return _mqttServer.StartAsync(optionsBuilder.Build());
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping MQTT Service.");

            return _mqttServer.StopAsync();
        }

        public void Dispose()
        {
            _logger.LogInformation("Disposing....");
        }

        private abstract class MqttServerClientHandler
        {
            protected readonly ILogger _logger;

            public MqttServerClientHandler(ILogger logger)
            {
                _logger = logger;
            }
        }

        private class MqttApplicationMessageReceivedHandler : MqttServerClientHandler, IMqttApplicationMessageReceivedHandler
        {
            public MqttApplicationMessageReceivedHandler(ILogger logger) : base(logger) { }

            public async Task HandleApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs e)
            {
                _logger.LogInformation(e.ClientId + " published message to topic " + e.ApplicationMessage.Topic);
            }
        }

        private class MqttServerClientConnectedHandler : MqttServerClientHandler, IMqttServerClientConnectedHandler
        {
            public MqttServerClientConnectedHandler(ILogger logger) : base(logger) { }

            public async Task HandleClientConnectedAsync(MqttServerClientConnectedEventArgs e)
            {
                _logger.LogInformation(e.ClientId + " Connected.");
            }
        }

        private class MqttServerClientDisconnectedHandler : MqttServerClientHandler, IMqttServerClientDisconnectedHandler
        {
            public MqttServerClientDisconnectedHandler(ILogger logger) : base(logger) { }

            public async Task HandleClientDisconnectedAsync(MqttServerClientDisconnectedEventArgs e)
            {
                _logger.LogInformation(e.ClientId + " Disonnected.");
            }
        }

        private class MqttServerClientSubscribedTopicHandler : MqttServerClientHandler, IMqttServerClientSubscribedTopicHandler
        {
            public MqttServerClientSubscribedTopicHandler(ILogger logger) : base(logger) { }

            public async Task HandleClientSubscribedTopicAsync(MqttServerClientSubscribedTopicEventArgs e)
            {
                _logger.LogInformation(e.ClientId + " subscribed to " + e.TopicFilter);
            }
        }

        private class MqttServerClientUnsubscribedTopicHandler : MqttServerClientHandler, IMqttServerClientUnsubscribedTopicHandler
        {
            public MqttServerClientUnsubscribedTopicHandler(ILogger logger) : base(logger) { }

            public async Task HandleClientUnsubscribedTopicAsync(MqttServerClientUnsubscribedTopicEventArgs e)
            {
                _logger.LogInformation(e.ClientId + " unsubscribed to " + e.TopicFilter);
            }
        }
    }
}
