using Contracts;

namespace TrafficInfoHttpApi.Services
{
    public interface IAirTrafficInfoService
    {
        AirTrafficInfoContract GetAirTrafficInfo();
        AirportContract GetAirport(string airportName);
        void UpdatePlane(PlaneContract planeContract);
        void UpdateAirport(AirportContract airportContract);
    }
}
