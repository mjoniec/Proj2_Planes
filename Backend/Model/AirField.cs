using System.Device.Location;

namespace Model
{
    public class AirField
    {
        public string Name { get; set; }

        public GeoCoordinate GeoCoordinate { get; set; }

        public AirField(string name, double latitude, double longitude)
        {
            Name = name;
            GeoCoordinate = new GeoCoordinate(latitude, longitude);
        }
    }
}
