using AirTrafficInfoContracts;
using System;
using System.Collections.Generic;

namespace MockAirTrafficinfoApi.Services
{
    public static class Navigation
    {
        /// <summary>
        /// Updates Latitude and Longitude by speed, destination airport amd time passed since last departure airport till now
        /// Based on: https://www.movable-type.co.uk/scripts/latlong.html#destPoint
        /// </summary>
        /// <param name="geoCoordinate"></param>
        public static void MovePlane(PlaneContract plane, DateTime currentTime)
        {
            //given:
            //  departure time and current time
            //  speed
            //  destination position and departure position

            //calculate:
            //  traveled distance
            //  bearing
            //  current position from departure position, distance and bearing

            //double lat1 = plane.DepartureAirport.Latitude;
            //double lon1 = plane.DepartureAirport.Longitude;
            double lat1 = plane.Latitude;
            double lon1 = plane.Longitude;
            double lat2 = plane.DestinationAirportLatitude;
            double lon2 = plane.DestinationAirportLongitude;

            double lat1Rad = ToRadians(lat1);
            double lon1Rad = ToRadians(lon1);
            double lat2Rad = ToRadians(lat2);
            double lon2Rad = ToRadians(lon2);

            //var travelDurationInMiliseconds = currentTime.Subtract(plane.DepartureTime).TotalMilliseconds;
            var travelDurationSinceLastUpdateInMiliseconds = currentTime.Subtract(plane.LastPositionUpdate).TotalMilliseconds;
            var distanceCoveredInMeters = plane.SpeedInMetersPerSecond * travelDurationSinceLastUpdateInMiliseconds / 1000;
            //var bearing = CalculateBearing(plane.DepartureAirport.Latitude, plane.DepartureAirport.Longitude, plane.DestinationAirport.Latitude, plane.DestinationAirport.Longitude);

            var bearing = BearingFromCoordinates(lat1Rad, lon1Rad, lat2Rad, lon2Rad);
            var position = CalculatePosition(lat1Rad, lon1Rad, bearing, distanceCoveredInMeters);

            plane.SymbolRotate = bearing;
            plane.Latitude = position[0];
            plane.Longitude = position[1];
        }

        private static double BearingFromCoordinates(double lat1Rad, double lon1Rad, double lat2Rad, double lon2Rad)
        {
            double dLonRad = (lon2Rad - lon1Rad);

            double y = Math.Sin(dLonRad) * Math.Cos(lat2Rad);
            double x = Math.Cos(lat1Rad) * Math.Sin(lat2Rad) - Math.Sin(lat1Rad)
                     * Math.Cos(lat2Rad) * Math.Cos(dLonRad);

            double brngRad = Math.Atan2(y, x);
            double brngDeg = ToDegrees(brngRad);

            double brng = (brngDeg + 360) % 360;

            return brng;
        }

        /// <summary>
        /// Calculate a new coordinate based on start, distance and bearing
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="bearing"></param>
        /// <param name="distance">in meters</param>
        /// <returns>list[latitude, longitude]</returns>
        private static List<double> CalculatePosition(double lat1Rad, double lon1Rad, double bearing, double distance)
        {
            var d = distance / 63710100.0; //Earth's radius in m
            var b = ToRadians(bearing);

            var lat2 = Math.Asin(Math.Sin(lat1Rad) * Math.Cos(d) +
                                 Math.Cos(lat1Rad) * Math.Sin(d) * Math.Cos(b));

            var lon2 = lon1Rad + Math.Atan2(Math.Sin(b) * Math.Sin(d) * Math.Cos(lat1Rad),
                              Math.Cos(d) - Math.Sin(lat1Rad) * Math.Sin(lat2));

            lon2 = ((lon2 + 3 * Math.PI) % (2 * Math.PI)) - Math.PI;

            return new List<double> { ToDegrees(lat2), ToDegrees(lon2) };
        }

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
