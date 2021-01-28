using AirTrafficInfoContracts;
using System;
using System.Collections.Generic;

namespace AirTrafficInfoServices
{
    public static class PlaneNavigation
    {
        /// <summary>
        /// Updates Latitude and Longitude by speed, destination airport amd time passed since last departure airport till now
        /// Based on: https://www.movable-type.co.uk/scripts/latlong.html#destPoint
        /// </summary>
        /// <param name="geoCoordinate"></param>
        public static void MovePlane(PlaneContract plane, DateTime currentTime)
        {
            //given:
            //  departure and current time
            //  speed
            //  destination and departure position

            //calculate:
            //  traveled distance
            //  bearing
            //  current position from departure distance and bearing

            var travelDurationInSeconds = currentTime.Subtract(plane.DepartureTime).TotalSeconds;
            var distanceCoveredInMeters = plane.SpeedInMetersPerSecond * travelDurationInSeconds;
            var bearing = CalculateBearing(plane.DepartureAirport.Latitude, plane.DepartureAirport.Longitude, plane.DestinationAirport.Latitude, plane.DestinationAirport.Longitude);
            var position = CalculatePosition(plane.DepartureAirport.Latitude, plane.DepartureAirport.Longitude, bearing, distanceCoveredInMeters);

            plane.SymbolRotate = bearing;//to be verified if 2 rotation angle is assignable directly from bearing
            plane.Latitude = position[0];
            plane.Longitude = position[1];
        }

        /// <summary>
        /// http://mathforum.org/library/drmath/view/55417.html
        /// https://stackoverflow.com/questions/3932502/calculate-angle-between-two-latitude-longitude-points
        /// https://www.cosmocode.de/en/blog/gohr/2010-06/29-calculate-a-destination-coordinate-based-on-distance-and-bearing-in-php
        /// </summary>
        /// <param name="lat1"></param>
        /// <param name="lon1"></param>
        /// <param name="lat2"></param>
        /// <param name="lon2"></param>
        /// <returns></returns>
        private static double CalculateBearing(double lat1, double lon1, double lat2, double lon2)
        {
            var y = Math.Sin(lon2 - lon1) * Math.Cos(lat2);
            var x = Math.Cos(lat1) * Math.Sin(lat2) - Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(lon2 - lon1);
            double bearing = 0.0;

            if (y > 0)
            {
                if (x > 0) bearing = Math.Atan(y / x);
                if (x < 0) bearing = 180.0 - Math.Atan(-y / x);
                if (x == 0) bearing = 90;
            }
            if (y < 0)
            {
                if (x > 0) bearing = -Math.Atan(-y / x);
                if (x < 0) bearing = Math.Atan(y / x) - 180;
                if (x == 0) bearing = 270;
            }
            if (y == 0)
            {
                if (x > 0) bearing = 0;
                if (x < 0) bearing = 180;
                if (x == 0) return 0; //the 2 points are the same
            }

            return bearing;
        }

        /// <summary>
        /// Calculate a new coordinate based on start, distance and bearing
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="bearing"></param>
        /// <param name="distance">in meters</param>
        /// <returns>list[latitude, longitude]</returns>
        private static List<double> CalculatePosition(double latitude, double longitude, double bearing, double distance)
        {
            var lat1 = ToRadians(latitude);
            var lon1 = ToRadians(longitude);
            var d = distance / 63710100.0; //Earth's radius in m
            var b = ToRadians(bearing);

            var lat2 = Math.Asin(Math.Sin(lat1) * Math.Cos(d) +
                  Math.Cos(lat1) * Math.Sin(d) * Math.Cos(b));

            var lon2 = lon1 + Math.Atan2(Math.Sin(b) * Math.Sin(d) * Math.Cos(lat1),
                          Math.Cos(d) - Math.Sin(lat1) * Math.Sin(lat2));

            lon2 = ((lon2 + 3 * Math.PI) % (2 * Math.PI)) - Math.PI;

            return new List<double> { ToDegrees(lat2), ToDegrees(lon2) };

            static double ToRadians(double degree)
            {
                return degree * Math.PI / 180;
            }

            static double ToDegrees(double radians)
            {
                return radians * 180 / Math.PI;
            }
        }
    }
}
