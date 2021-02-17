using AirTrafficInfoContracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MockAirTrafficinfoApi.Services
{
    public class AirTrafficInfoService
    {
        private readonly AirTrafficInfoContract _airTrafficInfoContract;
        
        public AirTrafficInfoService()
        {
            //USA
            var airport1 = new AirportContract
            {
                Name = "New York",
                Longitude = -74.17,
                Latitude = 40.68,
                Color = "#0000FF",
                IsGoodWeather = true
            };
            
            var airport2 = new AirportContract
            {
                Name = "Miami",
                Longitude = -80.17,
                Latitude = 25.78,
                Color = "#0b40f4",
                IsGoodWeather = true
            };

            var airport3 = new AirportContract
            {
                Name = "Los Angeles",
                Longitude = -118.4,
                Latitude = 33.93,
                Color = "#0a3bdb",
                IsGoodWeather = true
            };

            var airport4 = new AirportContract
            {
                Name = "San Francisco",
                Longitude = -122.41,
                Latitude = 37.78,
                Color = "#082eaa",
                IsGoodWeather = true
            };

            //Europe
            var airport5 = new AirportContract
            {
                Name = "London",
                Longitude = -0.11,
                Latitude = 51.48,
                Color = "#FF0000",
                IsGoodWeather = true
            };

            var airport6 = new AirportContract
            {
                Name = "Sevilla",
                Longitude = -5.98,
                Latitude = 37.37,
                Color = "#ff1a1a",
                IsGoodWeather = true
            };

            var airport7 = new AirportContract
            {
                Name = "Rome",
                Longitude = 12.5,
                Latitude = 41.89,
                Color = "#ff1a1a",
                IsGoodWeather = true
            };

            var airport8 = new AirportContract
            {
                Name = "Moscow",
                Longitude = 37.63,
                Latitude = 55.75,
                Color = "#ff3333",
                IsGoodWeather = true
            };

            //Asia
            var airport9 = new AirportContract
            {
                Name = "Tokyo",
                Longitude = 139.75,
                Latitude = 35.67,
                Color = "#ff00ff",
                IsGoodWeather = true
            };

            var airport10 = new AirportContract
            {
                Name = "Kuala Lumpur",
                Longitude = 101.68,
                Latitude = 3.13,
                Color = "#1affff",
                IsGoodWeather = true
            };

            //Middle East
            var airport11 = new AirportContract
            {
                Name = "Cairo",
                Longitude = 31.4,
                Latitude = 29.9,
                Color = "#ffff00",
                IsGoodWeather = true
            };

            var airport12 = new AirportContract
            {
                Name = "Dubai",
                Longitude = 55.2,
                Latitude = 25.3,
                Color = "#ffff33",
                IsGoodWeather = true
            };

            //Australia
            var airport13 = new AirportContract
            {
                Name = "Sydney",
                Longitude = 151.2,
                Latitude = -33.8,
                Color = "#c61aff",
                IsGoodWeather = true
            };

            //South America
            var airport14 = new AirportContract
            {
                Name = "Rio De Janeiro",
                Longitude = -43.2,
                Latitude = -22.9,
                Color = "#40ff00",
                IsGoodWeather = true
            };

            //Planes
            var planeContract1 = new PlaneContract
            {
                Name = "Maverick",
                Longitude = 0,
                Latitude = 0,
                SymbolRotate = 0,
                LastPositionUpdate = DateTime.Now,
                SpeedInMetersPerSecond = 1000000,
                DepartureAirportName = "New York",
                DestinationAirportName = "Miami"
            };

            var planeContract2 = new PlaneContract
            {
                Name = "Iceman",
                Longitude = 0,
                Latitude = 0,
                SymbolRotate = 0,
                LastPositionUpdate = DateTime.Now,
                SpeedInMetersPerSecond = 1000000,
                DepartureAirportName = "Miami",
                DestinationAirportName = "New York"
            };

            var planeContract3 = new PlaneContract
            {
                Name = "Slider",
                Longitude = 0,
                Latitude = 0,
                SymbolRotate = 0,
                LastPositionUpdate = DateTime.Now,
                SpeedInMetersPerSecond = 1000000,
                DepartureAirportName = "Miami",
                DestinationAirportName = "Los Angeles"
            };

            var planeContract4 = new PlaneContract
            {
                Name = "Goose",
                Longitude = 0,
                Latitude = 0,
                SymbolRotate = 0,
                LastPositionUpdate = DateTime.Now,
                SpeedInMetersPerSecond = 1000000,
                DepartureAirportName = "Miami",
                DestinationAirportName = "San Francisco"
            };

            var planeContract5 = new PlaneContract
            {
                Name = "Jester",
                Longitude = 0,
                Latitude = 0,
                SymbolRotate = 0,
                LastPositionUpdate = DateTime.Now,
                SpeedInMetersPerSecond = 1000000,
                DepartureAirportName = "Miami",
                DestinationAirportName = "London"
            };

            var planeContract6 = new PlaneContract
            {
                Name = "Viper",
                Longitude = 0,
                Latitude = 0,
                SymbolRotate = 0,
                LastPositionUpdate = DateTime.Now,
                SpeedInMetersPerSecond = 1000000,
                DepartureAirportName = "Miami",
                DestinationAirportName = "Sevilla"
            };

            var planeContract7 = new PlaneContract
            {
                Name = "Neo",
                Longitude = 0,
                Latitude = 0,
                SymbolRotate = 0,
                LastPositionUpdate = DateTime.Now,
                SpeedInMetersPerSecond = 1000000,
                DepartureAirportName = "Miami",
                DestinationAirportName = "Rome"
            };

            var planeContract8 = new PlaneContract
            {
                Name = "Gandalf",
                Longitude = 0,
                Latitude = 0,
                SymbolRotate = 0,
                LastPositionUpdate = DateTime.Now,
                SpeedInMetersPerSecond = 1000000,
                DepartureAirportName = "Miami",
                DestinationAirportName = "Moscow"
            };

            var planeContract9 = new PlaneContract
            {
                Name = "Aragorn",
                Longitude = 0,
                Latitude = 0,
                SymbolRotate = 0,
                LastPositionUpdate = DateTime.Now,
                SpeedInMetersPerSecond = 1000000,
                DepartureAirportName = "Miami",
                DestinationAirportName = "Tokyo"
            };

            _airTrafficInfoContract = new AirTrafficInfoContract
            {
                Airports = new List<AirportContract>
                {
                    airport1,
                    airport2,
                    airport3,
                    airport4,
                    airport5,
                    airport6,
                    airport7,
                    airport8,
                    airport9,
                    airport10,
                    airport11,
                    airport12,
                    airport13,
                    airport14
                },
                Planes = new List<PlaneContract>
                {
                    planeContract1,
                    planeContract2,
                    planeContract3,
                    planeContract4,
                    planeContract5,
                    planeContract6,
                    planeContract7,
                    planeContract8,
                    planeContract9,
                }
            };
        }

        internal AirTrafficInfoContract GetAirTrafficInfo()
        {
            return _airTrafficInfoContract;
        }

        internal void UpdateAirTrafficInfo()
        {
            _airTrafficInfoContract.Planes.ForEach(p => UpdatePlane(p));
        }

        private void UpdatePlane(PlaneContract planeContract)
        {
            var currentTime = DateTime.Now;

            Navigation.MovePlane(planeContract, currentTime);

            planeContract.LastPositionUpdate = currentTime;

            if (HasPlaneReachedItsDestination(planeContract))
            {
                SelectNewDestinationAirport(planeContract);
            }
        }

        private void SelectNewDestinationAirport(PlaneContract planeContract)
        {
            var airports = _airTrafficInfoContract.Airports;
            var randomDestinationAirport = SelectRandomAirportExceptTheOneProvided(airports, planeContract.DestinationAirportName);

            planeContract.SetNewDestinationAndDepartureAirports(randomDestinationAirport);
            planeContract.DepartureTime = DateTime.Now;
        }

        private AirportContract SelectRandomAirportExceptTheOneProvided(List<AirportContract> airports, string exceptThisAirportName)
        {
            var airportsWithoutException = new List<AirportContract>(airports);

            airportsWithoutException.RemoveAll(a => a.Name == exceptThisAirportName);

            return airportsWithoutException[new Random().Next(0, airportsWithoutException.Count)];
        }

        private bool HasPlaneReachedItsDestination(PlaneContract planeContract)
        {
            return (Math.Abs(planeContract.DestinationAirportLatitude - planeContract.Latitude) <= 1 &&
                Math.Abs(planeContract.DestinationAirportLongitude - planeContract.Longitude) <= 1);
        }

        //private void UpdatePlane(PlaneContract planeContract)
        //{
        //    if (planeContract.DestinationAirport.Latitude > planeContract.Latitude)
        //    {
        //        planeContract.Latitude++;
        //    }
        //    else if (planeContract.DestinationAirport.Latitude < planeContract.Latitude)
        //    {
        //        planeContract.Latitude--;
        //    }

        //    if (planeContract.DestinationAirport.Longitude > planeContract.Longitude)
        //    {
        //        planeContract.Longitude++;
        //    }
        //    else if (planeContract.DestinationAirport.Longitude < planeContract.Longitude)
        //    {
        //        planeContract.Longitude--;
        //    }

        //    if (planeContract.DestinationAirport.Latitude == planeContract.Latitude &&
        //        planeContract.DestinationAirport.Longitude == planeContract.Longitude)
        //    {
        //        SelectNewDestinationAirport(planeContract);
        //    }
        //}

        //private void SelectNewDestinationAirport(PlaneContract planeContract)
        //{
        //    var random = new Random();

        //    while (true)
        //    {
        //        var nextAirport = _airTrafficInfoContract.Airports[random
        //            .Next(0, _airTrafficInfoContract.Airports.Count)];

        //        if (
        //            planeContract.DestinationAirport == null || //no destination so we can assign whatever
        //            planeContract.DestinationAirport.Name != nextAirport.Name)
        //        {
        //            planeContract.DepartureAirport = planeContract.DestinationAirport;
        //            planeContract.DestinationAirport = nextAirport;

        //            break;
        //        }
        //    }
        //}
    }
}
