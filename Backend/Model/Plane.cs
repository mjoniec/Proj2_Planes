using System.Device.Location;

namespace Model
{
    public class Plane
    {
        public string Name { get; set; }

        public GeoCoordinate GeoCoordinate { get; set; }

        public GeoCoordinate Destination { get; set; }

        public Plane(string name, double latitude, double longitude)
        {
            Name = name;
            GeoCoordinate = new GeoCoordinate(latitude, longitude);
            GeoCoordinate.Speed = 10;
        }
    }
}

//Altitude Pobiera wysokość GeoCoordinatew metrach.
//Course Pobiera lub ustawia nagłówek w stopniach względem wartości true Północnego. Prawidłowy zakres obejmuje wartości z zakresu od 0,0 do 360,0 i Double.NaN, jeśli nie zdefiniowano nagłówka.
//Latitude Pobiera lub ustawia szerokość geograficzną GeoCoordinate.
//Longitude Pobiera lub ustawia długość geograficzną GeoCoordinate.
//Speed Pobiera lub ustawia szybkość w licznikach na sekundę.
