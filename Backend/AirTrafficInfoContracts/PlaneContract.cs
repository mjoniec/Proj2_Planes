using System;

namespace AirTrafficInfoContracts
{
    public class PlaneContract
    {
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Color { get; set; }
        public double SymbolRotate { get; set; }
        public string Symbol => "arrow";
        public DateTime PositionUpdateTime { get; set; }
        public double SpeedInMetersPerSecond { get; set; }
        public AirportContract DepartureAirport { get; set; }
        public AirportContract DestinationAirport { get; set; }
        public Type Type => Type.Plane;

        //remove that method from here to some extension
        public void SetSpeedFromKilometersPerHour(int speedInKmH)
        {
            SpeedInMetersPerSecond = speedInKmH * 3600 * 1000;
        }
    }
}
