namespace AirTrafficInfoContracts
{
    public static class PlaneContractExtension
    {
        public static void SetDepartureAirportData(this PlaneContract planeContract, AirportContract airportContract)
        {
            planeContract.DepartureAirportName = airportContract.Name;
            planeContract.DepartureAirportLatitude = airportContract.Latitude;
            planeContract.DepartureAirportLongitude = airportContract.Longitude;
        }

        public static void SetDestinationAirportData(this PlaneContract planeContract, AirportContract airportContract)
        {
            planeContract.DestinationAirportName = airportContract.Name;
            planeContract.DestinationAirportLatitude = airportContract.Latitude;
            planeContract.DestinationAirportLongitude = airportContract.Longitude;
            planeContract.Color = airportContract.Color;
        }

        public static void SetNewDestinationAndDepartureAirports(this PlaneContract planeContract, AirportContract newDestinationAirportContract)
        {
            planeContract.DepartureAirportName = planeContract.DestinationAirportName;
            planeContract.DepartureAirportLatitude = planeContract.DestinationAirportLatitude;
            planeContract.DepartureAirportLongitude = planeContract.DestinationAirportLongitude;

            planeContract.SetDestinationAirportData(newDestinationAirportContract);
        }

        public static void AddPositionToHistory(this PlaneContract planeContract, double latitude, double longitude)
        {
            if (planeContract.PreviousPositionsLatitude.Count >= planeContract.PositionsHistory)
            {
                planeContract.PreviousPositionsLatitude.RemoveAt(0);
                planeContract.PreviousPositionsLongitude.RemoveAt(0);
            }

            planeContract.PreviousPositionsLatitude.Add(latitude);
            planeContract.PreviousPositionsLongitude.Add(longitude);
        }
    }
}
