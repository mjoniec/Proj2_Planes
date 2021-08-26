using Contracts;
using System;
using System.Globalization;
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
        private const int BadWeatherOccurenceChanceLikeOneToThisConstValue = 10;
        private readonly TimeSpan BadWeatherDuration = TimeSpan.FromSeconds(BadWeatherDurationInSeconds);
        private DateTime _badWeatherOccurence;//we need to know when bad weather happened in order to set it back after 10 seconds

        //should I expose this according to DDD ? Anemic model shared across whole solution is not sth that I recall being recommended...
        public AirportContract AirportContract => _airportContract; 

        private readonly AirportContract _airportContract;

        public Airport(string name, string color, string latitude, string longitude)
        {
            _airportContract = new AirportContract
            {
                Name = name,
                Color = string.IsNullOrEmpty(color) ? "#" + new Random().Next(100000, 999999).ToString() : color,
                Latitude = string.IsNullOrEmpty(latitude) ? new Random().Next(-35, 35) : double.Parse(latitude, CultureInfo.InvariantCulture),
                Longitude = string.IsNullOrEmpty(longitude) ? new Random().Next(-35, 35) : double.Parse(longitude, CultureInfo.InvariantCulture)
            };
        }

        public async Task UpdateAirport()
        {
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
            var badWeather = GetBadWeatherAtRandom();

            if (badWeather)
            {
                _airportContract.IsGoodWeather = !badWeather;
                _badWeatherOccurence = DateTime.Now;
            }
        }

        private void TrySetGoodWeatherAfterSomeDurationOfBadWeather()
        {
            _airportContract.IsGoodWeather = DateTime.Now - _badWeatherOccurence > BadWeatherDuration;
        }

        private bool GetBadWeatherAtRandom()
        {
            return new Random().Next(1, BadWeatherOccurenceChanceLikeOneToThisConstValue + 1) == BadWeatherOccurenceChanceLikeOneToThisConstValue;
        }
    }
}
