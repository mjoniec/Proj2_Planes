using AirTrafficInfoContracts;
using AirTrafficInfoServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MockAirTrafficinfoApi.Services
{
    public class MockAirTrafficInfoService
    {
        private readonly AirTrafficInfoContract _airTrafficInfoContract;
        
        public MockAirTrafficInfoService()
        {

            _airTrafficInfoContract = AirTrafficInfoContractDataFactory.Get();
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
            var randomDestinationAirport = SelectRandomAirportWithException(airports, planeContract.DestinationAirportName);

            planeContract.SetNewDestinationAndDepartureAirports(randomDestinationAirport);
            planeContract.DepartureTime = DateTime.Now;
        }

        private AirportContract SelectRandomAirportWithException(List<AirportContract> airports, string exceptThisAirportName)
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
