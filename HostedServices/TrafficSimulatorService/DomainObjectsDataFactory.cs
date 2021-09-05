using AirportService.Domain;
using PlaneService.Domain;
using System.Collections.Generic;

namespace TrafficSimulatorService
{
    public static class DomainObjectsDataFactory
    {
        public static List<PlaneLifetimeManager> GetPlaneLifetimeManagers(
            string updatePlaneUrl, string addPlaneUrl, string getAirportUrl, string getAirportsUrl)
        {
            return new List<PlaneLifetimeManager>
            {
                new PlaneLifetimeManager("Maverick", updatePlaneUrl, addPlaneUrl, getAirportUrl, getAirportsUrl),
                new PlaneLifetimeManager("Iceman", updatePlaneUrl, addPlaneUrl, getAirportUrl, getAirportsUrl),
                new PlaneLifetimeManager("Slider", updatePlaneUrl, addPlaneUrl, getAirportUrl, getAirportsUrl),
                new PlaneLifetimeManager("Goose", updatePlaneUrl, addPlaneUrl, getAirportUrl, getAirportsUrl),
                new PlaneLifetimeManager("Jester", updatePlaneUrl, addPlaneUrl, getAirportUrl, getAirportsUrl),
                new PlaneLifetimeManager("Viper", updatePlaneUrl, addPlaneUrl, getAirportUrl, getAirportsUrl),
                new PlaneLifetimeManager("Neo", updatePlaneUrl, addPlaneUrl, getAirportUrl, getAirportsUrl),
                new PlaneLifetimeManager("Gandalf", updatePlaneUrl, addPlaneUrl, getAirportUrl, getAirportsUrl),
                new PlaneLifetimeManager("Aragorn", updatePlaneUrl, addPlaneUrl, getAirportUrl, getAirportsUrl)
            };
        }

        public static List<AirportLifetimeManager> GetAirportLifetimeManagers(string updateAirportUrl, string addAirportUrl)
        {
            return new List<AirportLifetimeManager>
            {
                new AirportLifetimeManager("New York", "#0000FF", 40.68.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture), (-74.17).ToString("0.##", System.Globalization.CultureInfo.InvariantCulture), updateAirportUrl, addAirportUrl),
                new AirportLifetimeManager("Miami", "#0b40f4", 25.78.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture), (-80.17).ToString("0.##", System.Globalization.CultureInfo.InvariantCulture), updateAirportUrl, addAirportUrl),
                new AirportLifetimeManager("Los Angeles", "#0a3bdb", 33.93.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture), (-118.4).ToString("0.##", System.Globalization.CultureInfo.InvariantCulture), updateAirportUrl, addAirportUrl),
                new AirportLifetimeManager("San Francisco", "#082eaa", 37.78.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture), (-122.41).ToString("0.##", System.Globalization.CultureInfo.InvariantCulture), updateAirportUrl, addAirportUrl),

                new AirportLifetimeManager("London", "#FF0000", 51.48.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture), (-0.11).ToString("0.##", System.Globalization.CultureInfo.InvariantCulture), updateAirportUrl, addAirportUrl),
                new AirportLifetimeManager("Sevilla", "#ff1a1a", 37.37.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture), (-5.98).ToString("0.##", System.Globalization.CultureInfo.InvariantCulture), updateAirportUrl, addAirportUrl),
                new AirportLifetimeManager("Rome", "#ff1a1a", 41.89.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture), 12.5.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture), updateAirportUrl, addAirportUrl),
                new AirportLifetimeManager("Moscow", "#ff3333", 55.75.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture), 37.63.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture), updateAirportUrl, addAirportUrl),

                new AirportLifetimeManager("Tokyo", "#ff00ff", 35.67.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture), 139.75.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture), updateAirportUrl, addAirportUrl),
                new AirportLifetimeManager("Kuala Lumpur", "#1affff", 3.13.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture), 101.68.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture), updateAirportUrl, addAirportUrl),
                new AirportLifetimeManager("Cairo", "#ffff00", 29.9.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture), 31.4.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture), updateAirportUrl, addAirportUrl),
                new AirportLifetimeManager("Dubai", "#ffff33", 25.3.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture), 55.2.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture), updateAirportUrl, addAirportUrl),

                new AirportLifetimeManager("Sydney", "#c61aff", (-33.8).ToString("0.##", System.Globalization.CultureInfo.InvariantCulture), 151.2.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture), updateAirportUrl, addAirportUrl),
                new AirportLifetimeManager("Rio De Janeiro", "#40ff00", (-22.9).ToString("0.##", System.Globalization.CultureInfo.InvariantCulture), (-43.2).ToString("0.##", System.Globalization.CultureInfo.InvariantCulture), updateAirportUrl, addAirportUrl)
            };
        }
    }
}
