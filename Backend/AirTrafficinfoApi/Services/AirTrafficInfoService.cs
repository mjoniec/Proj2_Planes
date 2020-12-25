using AirTrafficInfoContracts;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AirTrafficinfoApi.Services
{
    public class AirTrafficInfoService
    {
        private string _planeName;
        private double _latitude;
        private double _longitude;
        //private PlaneContract _planeContract = new PlaneContract();
        private readonly List<PlaneContract> _planes;

        public AirTrafficInfoService()
        {
            _planes = new List<PlaneContract>();
        }

        internal string GetAirTrafficInfo()
        {
            //return _planeName + " " + _latitude.ToString() + " " + _longitude.ToString();

            //return _planeContract.Name + " " + _planeContract.PositionX;

            var stringBuilder = new StringBuilder();

            foreach(var plane in _planes)
            {
                stringBuilder.AppendLine(plane.Name + " " + plane.PositionX);
            }

            return stringBuilder.ToString();
        }

        internal AirTrafficInfo GetAirTrafficInfoMock()
        {
            return new AirTrafficInfo
            {
                Planes = new List<Plane>
                {
                    new Plane
                    {
                        Name = "plane 1",
                        Position = new Position
                        {
                            Latitude = 10.0,
                            Longitude = 10.0
                        }
                    },
                    new Plane
                    {
                        Name = "plane 2",
                        Position = new Position
                        {
                            Latitude = 15.0,
                            Longitude = 15.0
                        }
                    }
                },
                Airports = new List<Airport>
                {
                    new Airport
                    {
                        Name = "airport 1",
                        Position = new Position
                        {
                            Latitude = 20.0,
                            Longitude = 20.0
                        }
                    },
                    new Airport
                    {
                        Name = "airport 2",
                        Position = new Position
                        {
                            Latitude = 25.0,
                            Longitude = 25.0
                        }
                    }
                }
            };
        }

        internal void UpdatePlaneInfo(Plane plane)
        {
            _planeName = plane.Name;
            _latitude = plane.Position.Latitude;
            _longitude = plane.Position.Longitude;
        }

        internal void UpdatePlaneInfo(PlaneContract planeContract)
        {
            if(!_planes.Any(p => p.Name == planeContract.Name))
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
