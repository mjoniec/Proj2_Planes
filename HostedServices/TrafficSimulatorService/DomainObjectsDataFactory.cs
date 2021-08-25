using AirportService.Domain;
using PlaneService.Domain;
using System.Collections.Generic;

namespace TrafficSimulatorService
{
    public static class DomainObjectsDataFactory
    {
        public static List<Plane> GetPlanes()
        {
            return new List<Plane>
            {
                new Plane("Maverick"),
                new Plane("Iceman"),
                new Plane("Slider"),
                new Plane("Goose"),
                new Plane("Jester"),
                new Plane("Viper"),
                new Plane("Neo"),
                new Plane("Gandalf"),
                new Plane("Aragorn")
            };
        }

        public static List<Airport> GetAirports()
        {
            return new List<Airport>
            {
                new Airport("New York",       "#0000FF", 40.68.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture), (-74.17).ToString("0.##", System.Globalization.CultureInfo.InvariantCulture)),
                new Airport("Miami",          "#0b40f4", 25.78.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture), (-80.17).ToString("0.##", System.Globalization.CultureInfo.InvariantCulture)),
                new Airport("Los Angeles",    "#0a3bdb", 33.93.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture), (-118.4).ToString("0.##", System.Globalization.CultureInfo.InvariantCulture)),
                new Airport("San Francisco",  "#082eaa", 37.78.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture), (-122.41).ToString("0.##", System.Globalization.CultureInfo.InvariantCulture)),
                                                                              
                new Airport("London",         "#FF0000", 51.48.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture), (-0.11).ToString("0.##", System.Globalization.CultureInfo.InvariantCulture)),
                new Airport("Sevilla",        "#ff1a1a", 37.37.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture), (-5.98).ToString("0.##", System.Globalization.CultureInfo.InvariantCulture)),
                new Airport("Rome",           "#ff1a1a", 41.89.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture), 12.5.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture)),
                new Airport("Moscow",         "#ff3333", 55.75.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture), 37.63.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture)),
                                              
                new Airport("Tokyo",          "#ff00ff", 35.67.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture), 139.75.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture)),
                new Airport("Kuala Lumpur",   "#1affff", 3.13.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture),  101.68.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture)),
                new Airport("Cairo",          "#ffff00", 29.9.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture), 31.4.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture)),
                new Airport("Dubai",          "#ffff33", 25.3.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture), 55.2.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture)),
                
                new Airport("Sydney",         "#c61aff", (-33.8).ToString("0.##", System.Globalization.CultureInfo.InvariantCulture), 151.2.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture)),
                new Airport("Rio De Janeiro", "#40ff00", (-22.9).ToString("0.##", System.Globalization.CultureInfo.InvariantCulture), (-43.2).ToString("0.##", System.Globalization.CultureInfo.InvariantCulture))
            };
        }
    }
}
