using AirTrafficInfoApi.Services;
using AirTrafficInfoContracts;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AirTrafficInfoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirTrafficInfoController : ControllerBase
    {
        private readonly AirTrafficInfoService _airTrafficInfoService;
        private readonly StaticResourcesProvider _staticResourcesProvider;

        public AirTrafficInfoController(AirTrafficInfoService airTrafficInfoService,
            StaticResourcesProvider staticResourcesService)
        {
            _airTrafficInfoService = airTrafficInfoService;
            _staticResourcesProvider = staticResourcesService;
        }

        [HttpGet]
        [EnableCors("MyAllowedOrigins")]
        public AirTrafficInfoContract Get()
        {
            return _airTrafficInfoService.GetAirTrafficInfo();
        }

        [HttpGet]
        [Route("GetAirports")]
        public List<AirportContract> GetAirports()
        {
            return _airTrafficInfoService.GetAirTrafficInfo().Airports;
        }

        [HttpPost]
        [Route("UpdatePlaneInfo")]
        public void UpdatePlaneInfo([FromBody] PlaneContract planeContract)
        {
            _airTrafficInfoService.UpdatePlane(planeContract);
        }

        [HttpPost]
        [Route("UpdateAirportInfo")]
        public void UpdateAirportInfo([FromBody] AirportContract airportContract)
        {
            _airTrafficInfoService.UpdateAirport(airportContract);
        }

        //https://localhost:44389/api/AirTrafficInfo/WorldMap
        [EnableCors("MyAllowedOrigins")]
        [HttpGet]
        [Route("WorldMap")]
        public async Task<string> WorldMap()
        {
            return await _staticResourcesProvider.GetWorldMap();
        }
    }
}
