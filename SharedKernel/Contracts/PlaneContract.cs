using System;
using System.Collections.Generic;

namespace Contracts
{
    public class PlaneContract
    {
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double SpeedInMetersPerSecond { get; set; }
        public DateTime LastPositionUpdate { get; set; }
        public DateTime DepartureTime { get; set; }

        public List<double> PreviousPositionsLatitude { get; set; }
        public List<double> PreviousPositionsLongitude { get; set; }

        //Airport copy
        public string DepartureAirportName { get; set; }
        public double DepartureAirportLatitude { get; set; }
        public double DepartureAirportLongitude { get; set; }
        public string DestinationAirportName { get; set; }
        public double DestinationAirportLatitude { get; set; }
        public double DestinationAirportLongitude { get; set; }

        //const
        public Type Type => Type.Plane; //this does not change hence could be readonly property
        public int PositionsHistory = 9; //this could go to refactor to be read from some config etc

        //UI related
        //public string Color => DestinationAirport.Color;
        public string Color { get; set; } // angular doesnt always refresh referencing properties - has to be poco with color info copied from targeted airport. 
        public double SymbolRotate { get; set; }//value should contain bearing angle calculated to mercator 2d map rotation
    }
}
