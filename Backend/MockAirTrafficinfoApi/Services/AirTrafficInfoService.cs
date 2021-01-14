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
                PositionX = 10,
                Positiony = 10,
                IsGoodWeather = true
            };
            
            var airport2 = new AirportContract
            {
                Name = "Airport 2",
                PositionX = 20,
                Positiony = 20,
                IsGoodWeather = true
            };

            _airTrafficInfoContract = new AirTrafficInfoContract
            {
                Airports = new List<AirportContract>
                {
                    airport1,
                    airport2
                },
                Planes = new List<PlaneContract>
                {
                    new PlaneContract
                    {
                        Name = "Plane 1",
                        PositionX = 11,
                        Positiony = 11,
                        PositionUpdateTime = DateTime.Now,
                        SpeedInMetersPerSecond = 1,
                        DepartureAirport = airport1,
                        DestinationAirport = airport2
                    },
                    new PlaneContract
                    {
                        Name = "Plane 2",
                        PositionX = 12,
                        Positiony = 12,
                        PositionUpdateTime = DateTime.Now,
                        SpeedInMetersPerSecond = 1,
                        DepartureAirport = airport1,
                        DestinationAirport = airport2
                    },
                    new PlaneContract
                    {
                        Name = "Plane 3",
                        PositionX = 13,
                        Positiony = 13,
                        PositionUpdateTime = DateTime.Now,
                        SpeedInMetersPerSecond = 1,
                        DepartureAirport = airport1,
                        DestinationAirport = airport2
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
            _airTrafficInfoContract.Planes.ForEach(p => p.PositionX += 1);
        }
    }
}
