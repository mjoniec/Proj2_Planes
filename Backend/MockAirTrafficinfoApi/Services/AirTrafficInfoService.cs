using AirTrafficInfoContracts;
using System;
using System.Collections.Generic;

namespace MockAirTrafficinfoApi.Services
{
    public class AirTrafficInfoService
    {
        private readonly AirTrafficInfoContract _airTrafficInfoContract;
        
        public AirTrafficInfoService()
        {
            var airport1 = new AirportContract
            {
                Name = "Airport 1",
                Longitude = -82,
                Latitude = 32,
                Color = "#FF1111",
                SymbolRotate = 45,
                IsGoodWeather = true
            };
            
            var airport2 = new AirportContract
            {
                Name = "Airport 2",
                Longitude = 10,
                Latitude = 52,
                Color = "#1111FF",
                SymbolRotate = 45,
                IsGoodWeather = true
            };

            var airport3 = new AirportContract
            {
                Name = "Airport 3",
                Longitude = -43,
                Latitude = -20,
                Color = "#11FF11",
                SymbolRotate = 45,
                IsGoodWeather = true
            };

            var planeContract1 = new PlaneContract
            {
                Name = "Plane 1",
                Longitude = 75,
                Latitude = 00,
                SymbolRotate = -90,
                LastPositionUpdate = DateTime.Now,
                SpeedInMetersPerSecond = 1,
                DepartureAirport = airport1,
                DestinationAirport = airport2
            };

            var planeContract2 = new PlaneContract
            {
                Name = "Plane 2",
                Longitude = 100,
                Latitude = 25,
                SymbolRotate = 180,
                LastPositionUpdate = DateTime.Now,
                SpeedInMetersPerSecond = 1,
                DepartureAirport = airport3,
                DestinationAirport = airport2
            };

            var planeContract3 = new PlaneContract
            {
                Name = "Plane 3",
                Longitude = 50,
                Latitude = 50,
                SymbolRotate = -45,
                LastPositionUpdate = DateTime.Now,
                SpeedInMetersPerSecond = 1,
                DepartureAirport = airport1,
                DestinationAirport = airport3
            };

            _airTrafficInfoContract = new AirTrafficInfoContract
            {
                Airports = new List<AirportContract>
                {
                    airport1,
                    airport2,
                    airport3
                },
                Planes = new List<PlaneContract>
                {
                    planeContract1,
                    planeContract2,
                    planeContract3
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
            if (planeContract.DestinationAirport.Latitude > planeContract.Latitude)
            {
                planeContract.Latitude++;
            }
            else if (planeContract.DestinationAirport.Latitude < planeContract.Latitude)
            {
                planeContract.Latitude--;
            }

            if (planeContract.DestinationAirport.Longitude > planeContract.Longitude)
            {
                planeContract.Longitude++;
            }
            else if (planeContract.DestinationAirport.Longitude < planeContract.Longitude)
            {
                planeContract.Longitude--;
            }

            if (planeContract.DestinationAirport.Latitude == planeContract.Latitude &&
                planeContract.DestinationAirport.Longitude == planeContract.Longitude)
            {
                SelectNewDestinationAirport(planeContract);
            }
        }

        private void SelectNewDestinationAirport(PlaneContract planeContract)
        {
            var random = new Random();

            while (true)
            {
                var nextAirport = _airTrafficInfoContract.Airports[random
                    .Next(0, _airTrafficInfoContract.Airports.Count)];

                if (
                    planeContract.DestinationAirport == null || //no destination so we can assign whatever
                    planeContract.DestinationAirport.Name != nextAirport.Name)
                {
                    planeContract.DepartureAirport = planeContract.DestinationAirport;
                    planeContract.DestinationAirport = nextAirport;

                    break;
                }
            }
        }
    }
}
