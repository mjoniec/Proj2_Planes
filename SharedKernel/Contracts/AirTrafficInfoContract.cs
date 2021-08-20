using System.Collections.Generic;

namespace Contracts
{
    public class AirTrafficInfoContract
    {
        public List<AirportContract> Airports { get; set; }
        public List<PlaneContract> Planes { get; set; }
    }
}
