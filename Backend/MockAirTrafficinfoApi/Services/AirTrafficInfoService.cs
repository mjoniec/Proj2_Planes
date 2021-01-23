using AirTrafficInfoContracts;
using System;
using System.Collections.Generic;

namespace MockAirTrafficinfoApi.Services
{
    public class AirTrafficInfoService
    {
        private readonly AirTrafficInfoContract _airTrafficInfoContract;
        private readonly string _name;

        public AirTrafficInfoService()
        {
            _name = "MockAirTrafficInfoName_" + new Random().Next(1001, 9999).ToString();

            var airport1 = new AirportContract
            {
                Name = "Airport 1",
                Longitude = 0,
                Latitude = 0,
                Color = "#FF1111",
                SymbolRotate = 45,
                IsGoodWeather = true
            };
            
            var airport2 = new AirportContract
            {
                Name = "Airport 2",
                Longitude = 100,
                Latitude = 0,
                Color = "#1111FF",
                SymbolRotate = 45,
                IsGoodWeather = true
            };

            var airport3 = new AirportContract
            {
                Name = "Airport 3",
                Longitude = 100,
                Latitude = 100,
                Color = "#11FF11",
                SymbolRotate = 45,
                IsGoodWeather = true
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
                    new PlaneContract
                    {
                        Name = "Plane 1",
                        Longitude = 75,
                        Latitude = 00,
                        SymbolRotate = -90,
                        PositionUpdateTime = DateTime.Now,
                        SpeedInMetersPerSecond = 1,
                        DepartureAirport = airport1,
                        DestinationAirport = airport2
                    },
                    new PlaneContract
                    {
                        Name = "Plane 2",
                        Longitude = 100,
                        Latitude = 25,
                        SymbolRotate = 180,
                        PositionUpdateTime = DateTime.Now,
                        SpeedInMetersPerSecond = 1,
                        DepartureAirport = airport3,
                        DestinationAirport = airport2
                    },
                    new PlaneContract
                    {
                        Name = "Plane 3",
                        Longitude = 50,
                        Latitude = 50,
                        SymbolRotate = -45,
                        PositionUpdateTime = DateTime.Now,
                        SpeedInMetersPerSecond = 1,
                        DepartureAirport = airport1,
                        DestinationAirport = airport3
                    }
                }
            };
        }

        internal AirTrafficInfoContract GetAirTrafficInfo()
        {
            return _airTrafficInfoContract;
        }

        internal void UpdateAirTrafficInfo()
        {
            _airTrafficInfoContract.Planes.ForEach(p => p.Longitude += 1);
        }
    }
}
