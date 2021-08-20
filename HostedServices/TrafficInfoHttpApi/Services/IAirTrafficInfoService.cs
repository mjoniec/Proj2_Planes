using Contracts;

namespace TrafficInfoHttpApi.Services
{
    public interface IAirTrafficInfoService
    {
        AirTrafficInfoContract GetAirTrafficInfo();
        void UpdatePlane(PlaneContract planeContract);
        void UpdateAirport(AirportContract airportContract);
    }
}
