﻿using AirTrafficInfoApi.Services;
using AirTrafficInfoContracts;
using System.Collections.Generic;
using System.Linq;

namespace AirTrafficInfoApi.Services
{
    public class AirTrafficInfoService : IAirTrafficInfoService
    {
        private readonly AirTrafficInfoContract _airTrafficInfoContract;

        public AirTrafficInfoService()
        {
            _airTrafficInfoContract = new AirTrafficInfoContract
            {
                Airports = new List<AirportContract>(),
                Planes = new List<PlaneContract>()
            };
        }

        public AirTrafficInfoContract GetAirTrafficInfo() => _airTrafficInfoContract;

        public void UpdatePlane(PlaneContract planeContract)
        {
            //if (!_airTrafficInfoContract.Planes.Any(p => p.Name == planeContract.Name))
            if (!_airTrafficInfoContract.Planes.Select(p => p.Name)
                .ToList()//copy needed for concurrent collection modification errors
                .Contains(planeContract.Name))
            {
                _airTrafficInfoContract.Planes.Add(planeContract);
            }
            else
            {
                var planeToUpdate = _airTrafficInfoContract.Planes.First(p => p.Name == planeContract.Name);

                //make a separate presentation model out of it?
                planeToUpdate.Latitude = planeContract.Latitude;
                planeToUpdate.Longitude = planeContract.Longitude;
                planeToUpdate.Color = planeContract.Color;
                planeToUpdate.SymbolRotate = planeContract.SymbolRotate;
            }
        }

        public void UpdateAirport(AirportContract airportContract)
        {
            //if (!_airTrafficInfoContract.Airports.Any(p => p.Name == airportContract.Name))
            if (!_airTrafficInfoContract.Airports.Select(a => a.Name)
                .ToList()//copy needed for concurrent collection modification errors
                .Contains(airportContract.Name))
            {
                _airTrafficInfoContract.Airports.Add(airportContract);
            }
            else
            {
                var airportToUpdate = _airTrafficInfoContract.Airports.First(p => p.Name == airportContract.Name);

                //new presentation model lat lon readonly as they do not change
                airportToUpdate.IsGoodWeather = airportContract.IsGoodWeather;
            }
        }
    }
}