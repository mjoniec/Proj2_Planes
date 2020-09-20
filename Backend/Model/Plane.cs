using System;
using System.Collections.Generic;

namespace Model
{
    public class Plane
    {
        public string Name { get; set; }
        public Position Position { get; set; }
        public Position Destination { get; set; }
        public Position Departure { get; set; }
        public Airport DestinationAirport { get; set; }
        public List<Position> PreviousPositions { get; set; }

        /// <summary>
        /// meters per second
        /// </summary>
        public double Speed { get; set; } 

        public void SetSpeedFromKilometersPerHour(int speedInKmH)
        {
            Speed = speedInKmH * 3600 * 1000;
        }

        public DateTime DepartureTime { get; set; }
    }
}
