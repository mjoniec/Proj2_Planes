using AirTrafficInfoContracts;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AirTrafficinfoApi.Services
{
    public class AirTrafficInfoService
    {
        private readonly List<PlaneContract> _planes;

        public AirTrafficInfoService()
        {
            _planes = new List<PlaneContract>();
        }

        internal string GetAirTrafficInfo()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("GetAirTrafficInfo: ");

            foreach (var plane in _planes)
            {
                stringBuilder.AppendLine(plane.Name + " " + plane.PositionX);
            }

            return stringBuilder.ToString();
        }

        internal void UpdatePlaneInfo(PlaneContract planeContract)
        {
            if (!_planes.Any(p => p.Name == planeContract.Name))
            {
                _planes.Add(planeContract);
            }
            else
            {
                var planeToUpdate = _planes.First(p => p.Name == planeContract.Name);

                planeToUpdate.PositionX = planeContract.PositionX;
            }
        }
    }
}
