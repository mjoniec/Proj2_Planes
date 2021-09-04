using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TrafficInfoHttpApi.Services
{
    public class TrafficInfoService : ITrafficInfoService
    {
        private readonly TrafficInfoContract _airTrafficInfoContract;

        public TrafficInfoService()
        {
            _airTrafficInfoContract = new TrafficInfoContract
            {
                Airports = new List<AirportContract>(),
                Planes = new List<PlaneContract>()
            };
        }

        public TrafficInfoContract GetTrafficInfo() => _airTrafficInfoContract;

        public AirportContract GetAirport(string airportName)
        {
            return _airTrafficInfoContract.Airports.FirstOrDefault(a => a.Name == airportName);
        }

        public void AddPlane(PlaneContract planeContract)
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
                throw new Exception(planeContract.Name + " already exists");
            }
        }

        public void UpdatePlane(PlaneContract planeContract)
        {
            if (!_airTrafficInfoContract.Planes.Select(p => p.Name)
                .ToList()//copy needed for concurrent collection modification errors
                .Contains(planeContract.Name))
            {
                throw new Exception(planeContract.Name + " does not exist");
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

        public void AddAirport(AirportContract airportContract)
        {
            if (!_airTrafficInfoContract.Airports.Select(a => a.Name)
                .ToList()//copy needed for concurrent collection modification errors
                .Contains(airportContract.Name))
            {
                _airTrafficInfoContract.Airports.Add(airportContract);
            }
            else
            {
                throw new Exception(airportContract.Name + " already exists");
            }
        }

        public void UpdateAirport(AirportContract airportContract)
        {
            if (!_airTrafficInfoContract.Airports.Select(a => a.Name)
                .ToList()//copy needed for concurrent collection modification errors
                .Contains(airportContract.Name))
            {
                throw new Exception(airportContract.Name + " does not exist");
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
