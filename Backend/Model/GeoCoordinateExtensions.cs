using System;
using System.Device.Location;

namespace Model
{
    public static class GeoCoordinateExtensions
    {
        public static void SetCourse(this GeoCoordinate geoCoordinate, GeoCoordinate destination)
        {
            geoCoordinate.Course = 0;
        }

        /// <summary>
        /// Updates Latitude and Longitude by speed, course amd time
        /// </summary>
        /// <param name="geoCoordinate"></param>
        public static void Move(this GeoCoordinate geoCoordinate, TimeSpan timeSpan)
        {
            var x = geoCoordinate.Speed;

            geoCoordinate.Latitude = 1;
            geoCoordinate.Longitude = 1;
        }
    }
}
