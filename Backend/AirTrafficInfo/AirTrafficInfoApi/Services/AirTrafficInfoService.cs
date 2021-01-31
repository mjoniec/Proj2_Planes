using AirTrafficInfoContracts;
using System.Collections.Generic;
using System.Linq;

namespace AirTrafficInfoApi.Services
{
    public class AirTrafficInfoService
    {
        private readonly List<PlaneContract> _planes;
        private readonly List<AirportContract> _airports;

        public AirTrafficInfoService()
        {
            _planes = new List<PlaneContract>();
            _airports = new List<AirportContract>();
        }

        internal AirTrafficInfoContract GetAirTrafficInfo() =>
            new AirTrafficInfoContract
            {
                Airports = _airports,
                Planes = _planes
            };
        
        public List<AirportContract> GetAirports() => _airports;

        internal void UpdatePlaneInfo(PlaneContract planeContract)
        {
            if (!_planes.Any(p => p.Name == planeContract.Name))
            {
                _planes.Add(planeContract);
            }
            else
            {
                var planeToUpdate = _planes.First(p => p.Name == planeContract.Name);

                //make a separate presentation model out of it?
                planeToUpdate.Latitude = planeContract.Latitude;
                planeToUpdate.Longitude = planeContract.Longitude;
                planeToUpdate.Color = planeContract.Color;
                planeToUpdate.SymbolRotate = planeContract.SymbolRotate;
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
                var airportToUpdate = _airports.First(p => p.Name == airportContract.Name);

                //new presentation model lat lon readonly as they do not change
                airportToUpdate.IsGoodWeather = airportContract.IsGoodWeather;
            }
        }
    }
}
