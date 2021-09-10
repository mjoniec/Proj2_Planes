using Contracts;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AirportService.Domain
{
    public class Airport
    {
        //these const values inderictly determine that in order for the application to work properly, meaning:
        // - not getting all airports unavailable at the same time
        // - not getting bad weather too fewer times to notice it
        // the invoker of the domain update method should not call it approximately more frequently than once every second
        // 1% chance every 1 second >> 100 seconds / 14 airports >> every 7 seconds some airport will go out of service for 10 seconds
        //but we also need to give planes the chance to reach their destination at the same time witnessing the bad weather and plane redirection
        // 1% chance every 10 seconds >> every 70 seconds some airport will go out of service for 10 seconds
        //so no less frequently than 10 seconds tick

        private const double BadWeatherDurationInSeconds = 10.0;
        private const int BadWeatherOccurenceChanceLikeOneToThisConstValue = 15;//on each update invoke
        private readonly TimeSpan BadWeatherDuration = TimeSpan.FromSeconds(BadWeatherDurationInSeconds);
        private DateTime _badWeatherOccurence;//we need to know when bad weather happened in order to set it back after 10 seconds

        //should I expose this according to DDD ? Anemic model shared across whole solution is not sth that I recall being recommended...
        public AirportContract AirportContract => _airportContract; 

        private readonly AirportContract _airportContract;
        private readonly ILogger<Airport> _logger;

        public Airport(string name, string color, string latitude, string longitude)
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });

            _logger = loggerFactory.CreateLogger<Airport>();
            _airportContract = AirportContractExtension.GetAirportContractWithValidatedOrDefaultValues(name, color, latitude, longitude);
        }

        public async Task UpdateAirport()
        {
            _logger.LogInformation("UpdateAirport: " + DateTime.Now.ToString("G"));

            if (_airportContract.IsGoodWeather)
            {
                TrySetBadWeather();
            }
            else
            {
                TrySetGoodWeatherAfterSomeDurationOfBadWeather();
            }
        }

        private void TrySetBadWeather()
        {
            _logger.LogInformation("TrySetBadWeather");

            var badWeatherHappened = GetBadWeatherAtRandom();

            if (badWeatherHappened)
            {
                _logger.LogInformation("badWeather");
                _airportContract.IsGoodWeather = !badWeatherHappened;
                _badWeatherOccurence = DateTime.Now;
            }
        }

        private void TrySetGoodWeatherAfterSomeDurationOfBadWeather()
        {
            _logger.LogInformation("TrySetGoodWeatherAfterSomeDurationOfBadWeather");

            _airportContract.IsGoodWeather = DateTime.Now - _badWeatherOccurence > BadWeatherDuration;
        }

        private bool GetBadWeatherAtRandom()
        {
            _logger.LogInformation("GetBadWeatherAtRandom");

            //return false; //TODO temporarly turned off bad weather occurence
            return new Random().Next(1, BadWeatherOccurenceChanceLikeOneToThisConstValue + 1) == BadWeatherOccurenceChanceLikeOneToThisConstValue;
        }
    }
}
