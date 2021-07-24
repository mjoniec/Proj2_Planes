using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherInfo
{
    class OldPlaneService
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

                    //adds client to reveive messages
                    services.AddSingleton<IMqttClientSubscriber, MqttClientSubscriber>();

                    //adds service to run plane logic 
                    services.AddHostedService<PlaneService>();
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    //logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                });

            await builder.RunConsoleAsync();
        }
    }

    public class PlaneService : BackgroundService
    {
        private readonly ILogger<PlaneService> _logger;
        private readonly IOptions<Airport> _config;
        private readonly IMqttClientSubscriber _mqttClientSubscriber;
        private readonly HttpClient _httpClient = new HttpClient();

        private Plane _plane = new Plane
        {
            Name = "plane test 1 ",
            Speed = 10000.0
        };

        public PlaneService(
            ILogger<PlaneService> logger,
            IOptions<Airport> config,
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
                _logger.LogInformation($"Plane " + _plane.Name + " position: " + _plane.Position.Latitude + " " + _plane.Position.Longitude);

                //MovePlane();
                MovePlane(_plane, DateTime.Now);
                await NotifyAirTrafficApi();

                await Task.Delay(1000, stoppingToken);
            }
        }

        private async Task NotifyAirTrafficApi()
        {
            //TODO: take URL from appsettings or inject through docker somehow
            var response = await _httpClient.PostAsync(
                $"https://localhost:44389/api/airtrafficinfo",
                new StringContent(JsonConvert.SerializeObject(_plane), Encoding.UTF8, "application/json"));
        }

        private void ChangeDirectionPlane()
        {
            _plane.Departure.Latitude = 52.0;
            _plane.Departure.Longitude = 19.0;
            _plane.Destination.Latitude = 26.0;
            _plane.Destination.Longitude = -80.0;
            _plane.DepartureTime = DateTime.Now.AddSeconds(-3.0);
        }

        public void RequestReceivedHandler(object sender, MessageEventArgs e)
        {
            _logger.LogInformation("Plane service received message: " + e.Message);

            //some if for to turn plane or not
            //ChangeDirectionPlane();
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

        /// <summary>
        /// Updates Latitude and Longitude by speed, course amd time
        /// Based on: https://www.movable-type.co.uk/scripts/latlong.html#destPoint
        /// </summary>
        /// <param name="geoCoordinate"></param>
        private static void MovePlane(Plane plane, DateTime currentTime)
        {
            //given:
            //  departure and current time
            //  speed
            //  destination and departure position

            //calculate:
            //  traveled distance
            //  bearing
            //  current position from departure distance and bearing

            var travelDurationInSeconds = currentTime.Subtract(plane.DepartureTime).TotalSeconds;
            var distanceCoveredInMeters = plane.Speed * travelDurationInSeconds;
            var bearing = CalculateBearing(plane.Departure.Latitude, plane.Departure.Longitude, plane.Destination.Latitude, plane.Destination.Longitude);
            var position = CalculatePosition(plane.Departure.Latitude, plane.Departure.Longitude, bearing, distanceCoveredInMeters);

            plane.Position.Latitude = position[0];
            plane.Position.Longitude = position[1];
        }

        /// <summary>
        /// http://mathforum.org/library/drmath/view/55417.html
        /// https://stackoverflow.com/questions/3932502/calculate-angle-between-two-latitude-longitude-points
        /// https://www.cosmocode.de/en/blog/gohr/2010-06/29-calculate-a-destination-coordinate-based-on-distance-and-bearing-in-php
        /// </summary>
        /// <param name="lat1"></param>
        /// <param name="lon1"></param>
        /// <param name="lat2"></param>
        /// <param name="lon2"></param>
        /// <returns></returns>
        private static double CalculateBearing(double lat1, double lon1, double lat2, double lon2)
        {
            var y = Math.Sin(lon2 - lon1) * Math.Cos(lat2);
            var x = Math.Cos(lat1) * Math.Sin(lat2) - Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(lon2 - lon1);
            double bearing = 0.0;

            if (y > 0)
            {
                if (x > 0) bearing = Math.Atan(y / x);
                if (x < 0) bearing = 180.0 - Math.Atan(-y / x);
                if (x == 0) bearing = 90;
            }
            if (y < 0)
            {
                if (x > 0) bearing = -Math.Atan(-y / x);
                if (x < 0) bearing = Math.Atan(y / x) - 180;
                if (x == 0) bearing = 270;
            }
            if (y == 0)
            {
                if (x > 0) bearing = 0;
                if (x < 0) bearing = 180;
                if (x == 0) return 0; //the 2 points are the same
            }

            return bearing;
        }

        /// <summary>
        /// Calculate a new coordinate based on start, distance and bearing
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lon"></param>
        /// <param name="bearing"></param>
        /// <param name="distance">in meters</param>
        private static List<double> CalculatePosition(double lat, double lon, double bearing, double distance)
        {
            var lat1 = ToRadians(lat);
            var lon1 = ToRadians(lon);
            var d = distance / 63710100.0; //Earth's radius in m
            var b = ToRadians(bearing);

            var lat2 = Math.Asin(Math.Sin(lat1) * Math.Cos(d) +
                  Math.Cos(lat1) * Math.Sin(d) * Math.Cos(b));

            var lon2 = lon1 + Math.Atan2(Math.Sin(b) * Math.Sin(d) * Math.Cos(lat1),
                          Math.Cos(d) - Math.Sin(lat1) * Math.Sin(lat2));

            lon2 = ((lon2 + 3 * Math.PI) % (2 * Math.PI)) - Math.PI;

            return new List<double> { ToDegrees(lat2), ToDegrees(lon2) };

            static double ToRadians(double degree)
            {
                return degree * Math.PI / 180;
            }

            static double ToDegrees(double radians)
            {
                return radians * 180 / Math.PI;
            }
        }
    }
}
