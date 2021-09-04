using Contracts;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrafficInfoHttpApi.Services;

namespace TrafficInfoHttpApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrafficInfoController : ControllerBase
    {
        private readonly TrafficInfoService _trafficInfoService;
        private readonly StaticResourcesProvider _staticResourcesProvider;

        public TrafficInfoController(TrafficInfoService airTrafficInfoService,
            StaticResourcesProvider staticResourcesService)
        {
            _trafficInfoService = airTrafficInfoService;
            _staticResourcesProvider = staticResourcesService;
        }

        [HttpGet]
        [EnableCors("MyAllowedOrigins")]
        public TrafficInfoContract Get()
        {
            return _trafficInfoService.GetTrafficInfo();
        }

        [HttpGet]
        [EnableCors("MyAllowedOrigins")]
        [Route("GetAirport/{airportName}")]
        public AirportContract GetAirport(string airportName)
        {
            return _trafficInfoService.GetAirport(airportName);
        }

        [HttpGet]
        [EnableCors("MyAllowedOrigins")]
        [Route("GetAirports")]
        public List<AirportContract> GetAirports()
        {
            return _trafficInfoService.GetTrafficInfo().Airports;
        }

        [HttpPost]
        [EnableCors("MyAllowedOrigins")]
        [Route("AddPlane")]
        public void AddPlane([FromBody] PlaneContract planeContract)
        {
            _trafficInfoService.AddPlane(planeContract);
        }

        [HttpPost]
        [EnableCors("MyAllowedOrigins")]
        [Route("AddAirport")]
        public void AddAirport([FromBody] AirportContract airportContract)
        {
            _trafficInfoService.AddAirport(airportContract);
        }

        [HttpPut]
        [EnableCors("MyAllowedOrigins")]
        [Route("UpdatePlane")]
        public void UpdatePlane([FromBody] PlaneContract planeContract)//refactor to name and position
        {
            _trafficInfoService.UpdatePlane(planeContract);
        }

        [HttpPut]
        [EnableCors("MyAllowedOrigins")]
        [Route("UpdateAirport")]
        public void UpdateAirport([FromBody] AirportContract airportContract)//refactor to name and weather flag
        {
            _trafficInfoService.UpdateAirport(airportContract);
        }

        //remove after fix https://github.com/mjoniec/Proj2_Planes/issues/17
        //https://localhost:44389/api/AirTrafficInfo/WorldMap
        [EnableCors("MyAllowedOrigins")]
        [HttpGet("WorldMap")]
        public async Task<string> WorldMap()
        {
            return await _staticResourcesProvider.GetWorldMap();
        }
    }
}
