using Domain;
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
                new Airport("New York",       "#0000FF", 40.68.ToString(), (-74.17).ToString()),
                new Airport("Miami",          "#0b40f4", 25.78.ToString(), (-80.17).ToString()),
                new Airport("Los Angeles",    "#0a3bdb", 33.93.ToString(), (-118.4).ToString()),
                new Airport("San Francisco",  "#082eaa", 37.78.ToString(), (-122.41).ToString()),
                                              
                new Airport("London",         "#FF0000", 51.48.ToString(), (-0.11).ToString()),
                new Airport("Sevilla",        "#ff1a1a", 37.37.ToString(), (-5.98).ToString()),
                new Airport("Rome",           "#ff1a1a", 41.89.ToString(), 12.5.ToString()),
                new Airport("Moscow",         "#ff3333", 55.75.ToString(), 37.63.ToString()),
                                              
                new Airport("Tokyo",          "#ff00ff", 35.67.ToString(), 139.75.ToString()),
                new Airport("Kuala Lumpur",   "#1affff", 3.13.ToString(),  101.68.ToString()),
                new Airport("Cairo",          "#ffff00", 29.9.ToString(), 31.4.ToString()),
                new Airport("Dubai",          "#ffff33", 25.3.ToString(), 55.2.ToString()),
                
                new Airport("Sydney",         "#c61aff", (-33.8).ToString(), 151.2.ToString()),
                new Airport("Rio De Janeiro", "#40ff00", (-22.9).ToString(), (-43.2).ToString())
            };
        }
    }
}
