using Contracts;

namespace TrafficInfoApi.Services
{
    public interface ITrafficInfoService
    {
        TrafficInfoContract GetTrafficInfo();
        AirportContract GetAirport(string airportName);
        void AddPlane(PlaneContract planeContract);
        void UpdatePlane(PlaneContract planeContract);
        void AddAirport(AirportContract airportContract);
        void UpdateAirport(AirportContract airportContract);
    }
}
