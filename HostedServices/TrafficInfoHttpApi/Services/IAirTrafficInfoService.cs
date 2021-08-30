using Contracts;

namespace TrafficInfoHttpApi.Services
{
    public interface IAirTrafficInfoService
    {
        AirTrafficInfoContract GetAirTrafficInfo();
        AirportContract GetAirport(string airportName);
        void AddPlane(PlaneContract planeContract);
        void UpdatePlane(PlaneContract planeContract);
        void AddAirport(AirportContract airportContract);
        void UpdateAirport(AirportContract airportContract);
    }
}
