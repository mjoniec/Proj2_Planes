using System;

namespace AirTrafficInfoContracts
{
    public class PlaneContract
    {
        public string Name { get; set; }
        public double PositionX { get; set; }
        public double Positiony { get; set; }
        public DateTime PositionUpdateTime { get; set; }
        public double SpeedInMetersPerSecond { get; set; }
        public AirportContract DepartureAirport { get; set; }
        public AirportContract DestinationAirport { get; set; }
        public Type Type => Type.Plane;

        public void SetSpeedFromKilometersPerHour(int speedInKmH)
        {
            SpeedInMetersPerSecond = speedInKmH * 3600 * 1000;
        }
    }
}
