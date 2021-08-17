using AirTrafficInfoContracts;

namespace AirTrafficInfoApi.Services
{
    public interface IAirTrafficInfoService
    {
        AirTrafficInfoContract GetAirTrafficInfo();
        void UpdatePlane(PlaneContract planeContract);
        void UpdateAirport(AirportContract airportContract);
    }
}
