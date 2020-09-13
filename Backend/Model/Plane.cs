using System;

namespace Model
{
    public class Plane
    {
        public string Name { get; set; }
        public double PositionLatitude { get; set; }
        public double PositionLongitude { get; set; }
        public double DestinationLatitude { get; set; }
        public double DestinationLongitude { get; set; }
        public double DepartureLatitude { get; set; }
        public double DepartureLongitude { get; set; }

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
