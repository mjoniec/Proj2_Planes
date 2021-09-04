using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TrafficInfoApi.Services
{
    public class TrafficInfoService : ITrafficInfoService
    {
        private readonly TrafficInfoContract _trafficInfoContract;

        public TrafficInfoService()
        {
            _trafficInfoContract = new TrafficInfoContract
            {
                Airports = new List<AirportContract>(),
                Planes = new List<PlaneContract>()
            };
        }

        public TrafficInfoContract GetTrafficInfo() => _trafficInfoContract;

        public AirportContract GetAirport(string airportName)
        {
            return _trafficInfoContract.Airports.FirstOrDefault(a => a.Name == airportName);
        }

        public void AddPlane(PlaneContract planeContract)
        {
            //tolist local copy needed for concurrent collection modification errors
            //if (!_TrafficInfoContract.Planes.Any(p => p.Name == planeContract.Name))
            if (!_trafficInfoContract.Planes.Select(p => p.Name)
                .ToList()
                .Contains(planeContract.Name))
            {
                _trafficInfoContract.Planes.Add(planeContract);
            }
            else
            {
                throw new Exception(planeContract.Name + " already exists");
            }
        }

        public void UpdatePlane(PlaneContract planeContract)
        {
            if (!_trafficInfoContract.Planes.Select(p => p.Name)
                .ToList()
                .Contains(planeContract.Name))
            {
                throw new Exception(planeContract.Name + " does not exist");
            }
            else
            {
                var planeToUpdate = _trafficInfoContract.Planes.First(p => p.Name == planeContract.Name);

                //make a separate presentation model out of it?
                planeToUpdate.Latitude = planeContract.Latitude;
                planeToUpdate.Longitude = planeContract.Longitude;
                planeToUpdate.Color = planeContract.Color;
                planeToUpdate.SymbolRotate = planeContract.SymbolRotate;
            }
        }

        public void AddAirport(AirportContract airportContract)
        {
            if (!_trafficInfoContract.Airports.Select(a => a.Name)
                .ToList()
                .Contains(airportContract.Name))
            {
                _trafficInfoContract.Airports.Add(airportContract);
            }
            else
            {
                throw new Exception(airportContract.Name + " already exists");
            }
        }

        public void UpdateAirport(AirportContract airportContract)
        {
            if (!_trafficInfoContract.Airports.Select(a => a.Name)
                .ToList()
                .Contains(airportContract.Name))
            {
                throw new Exception(airportContract.Name + " does not exist");
            }
            else
            {
                var airportToUpdate = _trafficInfoContract.Airports.First(p => p.Name == airportContract.Name);

                //new presentation model lat lon readonly as they do not change
                airportToUpdate.IsGoodWeather = airportContract.IsGoodWeather;
            }
        }
    }
}
