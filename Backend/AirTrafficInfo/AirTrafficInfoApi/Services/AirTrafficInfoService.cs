using AirTrafficInfoContracts;
using System.Collections.Generic;
using System.Linq;

namespace AirTrafficInfoApi.Services
{
    public class AirTrafficInfoService
    {
        private List<PlaneContract> Planes => _airTrafficInfoContract.Planes;
        private List<AirportContract> Airports => _airTrafficInfoContract.Airports;
        private readonly AirTrafficInfoContract _airTrafficInfoContract;

        public AirTrafficInfoService()
        {
            _airTrafficInfoContract = new AirTrafficInfoContract
            {
                Airports = new List<AirportContract>(),
                Planes = new List<PlaneContract>()
            };
        }

        internal AirTrafficInfoContract GetAirTrafficInfo() =>
            new AirTrafficInfoContract
            {
                Airports = Airports,
                Planes = Planes
            };
        
        public List<AirportContract> GetAirports() => Airports;

        internal void UpdatePlaneInfo(PlaneContract planeContract)
        {
            if (!Planes.Any(p => p.Name == planeContract.Name))
            {
                Planes.Add(planeContract);
            }
            else
            {
                var planeToUpdate = Planes.First(p => p.Name == planeContract.Name);

                //make a separate presentation model out of it?
                planeToUpdate.Latitude = planeContract.Latitude;
                planeToUpdate.Longitude = planeContract.Longitude;
                planeToUpdate.Color = planeContract.Color;
                planeToUpdate.SymbolRotate = planeContract.SymbolRotate;
            }
        }

        internal void UpdateAirportInfo(AirportContract airportContract)
        {
            if (!Airports.Any(p => p.Name == airportContract.Name))
            {
                Airports.Add(airportContract);
            }
            else
            {
                var airportToUpdate = Airports.First(p => p.Name == airportContract.Name);

                //new presentation model lat lon readonly as they do not change
                airportToUpdate.IsGoodWeather = airportContract.IsGoodWeather;
            }
        }
    }
}
