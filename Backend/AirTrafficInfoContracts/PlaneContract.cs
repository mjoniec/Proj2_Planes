using System;

namespace AirTrafficInfoContracts
{
    public class PlaneContract
    {
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime LastPositionUpdate { get; set; }
        public DateTime DepartureTime { get; set; }
        public double SpeedInMetersPerSecond { get; set; }
        public AirportContract DepartureAirport { get; set; }
        public AirportContract DestinationAirport { get; set; }
        public Type Type => Type.Plane;

        //UI related
        public string Color => DestinationAirport.Color;
        public double SymbolRotate { get; set; }
        public string Symbol => "arrow";
    }
}
