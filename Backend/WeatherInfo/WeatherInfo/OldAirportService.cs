using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherInfo
{
    class OldAirportService
    {
    }

    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = new HostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    //for some reason on debug it acts as in production, see proj/env vars #22
                    config.AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: false);
                    config.AddEnvironmentVariables();

                    if (args != null)
                    {
                        config.AddCommandLine(args);
                    }
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddOptions();
                    services.Configure<MqttConfig>(hostContext.Configuration.GetSection("Mqtt"));//same section for server and client

                    //adds mqtt server functionality
                    services.AddSingleton<IHostedService, MqttService>();

                    //adds client to publish messages
                    services.AddSingleton<IMqttClientPublisher, MqttClientPublisher>();

                    //adds service to run airport logic 
                    services.AddHostedService<AirportService>();
                })
                .ConfigureLogging((hostingContext, logging) => {
                    //logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                });

            await builder.RunConsoleAsync();
        }
    }

    public class AirportService : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly IOptions<Airport> _config;
        private readonly IMqttClientPublisher _mqttClientPublisher;

        public AirportService(
            ILogger<Airport> logger,
            IOptions<Airport> config,
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

                //ChangeWeather();

                await Task.Delay(14000, stoppingToken);
            }
        }

        private void ChangeWeather()
        {
            _mqttClientPublisher.PublishAsync("Wheather status change for airport: " + "airport test 1");
        }

        private void PingAliveStatus()
        {
            _mqttClientPublisher.PublishAsync(_config.Value.Name + " airport alive");
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
