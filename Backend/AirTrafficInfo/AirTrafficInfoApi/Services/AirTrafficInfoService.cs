using AirTrafficInfoContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AirTrafficInfoApi.Services
{
    public class AirTrafficInfoService
    {
        private readonly List<PlaneContract> _planes;
        private readonly List<AirportContract> _airports;
        private readonly string _name;

        public AirTrafficInfoService()
        {
            _planes = new List<PlaneContract>();
            _airports = new List<AirportContract>();
            _name = "AirTrafficInfoName_" + new Random().Next(1001, 9999).ToString();
        }

        internal string GetAirTrafficInfo()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine(_name);

            foreach (var plane in _planes)
            {
                stringBuilder.AppendLine(plane.Name + " " + plane.PositionX);
            }

            foreach (var airport in _airports)
            {
                stringBuilder.AppendLine(airport.Name + " " + airport.PositionX);
            }

            return stringBuilder.ToString();
        }

        internal void UpdatePlaneInfo(PlaneContract planeContract)
        {
            if (!_planes.Any(p => p.Name == planeContract.Name))
            {
                _planes.Add(planeContract);
            }
            else
            {
                var planeToUpdate = _planes.First(p => p.Name == planeContract.Name);

                planeToUpdate.PositionX = planeContract.PositionX;
            }
        }

        internal void UpdateAirportInfo(AirportContract airportContract)
        {
            if (!_airports.Any(p => p.Name == airportContract.Name))
            {
                _airports.Add(airportContract);
            }
            else
            {
                var planeToUpdate = _airports.First(p => p.Name == airportContract.Name);

                planeToUpdate.PositionX = airportContract.PositionX;
            }
        }
    }
}
