using System.Collections.Generic;

namespace AirTrafficInfoContracts
{
    public class AirTrafficInfoContract
    {
        public List<AirportContract> Airports { get; set; }
        public List<PlaneContract> Planes { get; set; }
    }
}
