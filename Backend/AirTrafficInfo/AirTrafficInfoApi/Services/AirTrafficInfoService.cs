using AirTrafficInfoContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AirTrafficInfoApi.Services
{
    public class AirTrafficInfoService
    {
        private readonly List<PlaneContract> _planes;
        private readonly string _name;

        public AirTrafficInfoService()
        {
            _planes = new List<PlaneContract>();
            _name = "AirTrafficInfoName_" + new Random().Next(1001, 9999).ToString();
        }

        internal string GetAirTrafficInfo()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine(_name);

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
