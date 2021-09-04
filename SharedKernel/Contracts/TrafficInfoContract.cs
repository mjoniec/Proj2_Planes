using System.Collections.Generic;

namespace Contracts
{
    public class TrafficInfoContract
    {
        public List<AirportContract> Airports { get; set; }
        public List<PlaneContract> Planes { get; set; }
    }
}
