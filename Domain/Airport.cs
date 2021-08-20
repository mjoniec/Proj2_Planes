using Contracts;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Domain
{
    public class Airport
    {
        private readonly AirportContract _airportContract;

        public AirportContract AirportContract => _airportContract; //TODO should I expose this according to DDD ?

        public Airport(string name, string color, string latitude, string longitude)
        {
            _airportContract = new AirportContract
            {
                Name = name,
                Color = string.IsNullOrEmpty(color) ? "#" + new Random().Next(100000, 999999).ToString() : color,
                Latitude = string.IsNullOrEmpty(latitude) ? new Random().Next(-150, 170) : double.Parse(latitude, CultureInfo.InvariantCulture),
                Longitude = string.IsNullOrEmpty(longitude) ? new Random().Next(-55, 70) : double.Parse(longitude, CultureInfo.InvariantCulture)
            };
        }

        public async Task UpdateAirport()
        {
            //weather related todo
        }
    }
}
