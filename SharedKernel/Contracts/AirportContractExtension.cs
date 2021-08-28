using System;
using System.Globalization;

namespace Contracts
{
    public static class AirportContractExtension
    {
        public static AirportContract GetAirportContractWithValidatedOrDefaultValues(
            string name, 
            string color, 
            string latitude,
            string longitude)
        {
            return new AirportContract
            {
                Name = name,
                Color = string.IsNullOrEmpty(color) ? "#" + new Random().Next(100000, 999999).ToString() : color,
                Latitude = string.IsNullOrEmpty(latitude) ? new Random().Next(-35, 35) : double.Parse(latitude, CultureInfo.InvariantCulture),
                Longitude = string.IsNullOrEmpty(longitude) ? new Random().Next(-35, 35) : double.Parse(longitude, CultureInfo.InvariantCulture)
            };
        }
    }
}
