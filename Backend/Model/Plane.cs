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

        //for now let it be diff from starting position - destination / some number that will get added after multiplying be time interval
        public double Speed { get; set; }
        public DateTime DepartureTime { get; set; }
    }
}

//Altitude Pobiera wysokość GeoCoordinatew metrach.
//Course Pobiera lub ustawia nagłówek w stopniach względem wartości true Północnego. Prawidłowy zakres obejmuje wartości z zakresu od 0,0 do 360,0 i Double.NaN, jeśli nie zdefiniowano nagłówka.
//Latitude Pobiera lub ustawia szerokość geograficzną GeoCoordinate.
//Longitude Pobiera lub ustawia długość geograficzną GeoCoordinate.
//Speed Pobiera lub ustawia szybkość w licznikach na sekundę.
