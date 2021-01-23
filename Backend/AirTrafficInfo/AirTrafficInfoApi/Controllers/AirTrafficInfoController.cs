using AirTrafficInfoApi.Services;
using AirTrafficInfoContracts;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace AirTrafficInfoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirTrafficInfoController : ControllerBase
    {
        private readonly AirTrafficInfoService _airTrafficInfoService;

        public AirTrafficInfoController(AirTrafficInfoService airTrafficInfoService)
        {
            _airTrafficInfoService = airTrafficInfoService;
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
            return _airTrafficInfoService.GetAirports();
        }

        [HttpPost]
        [Route("UpdatePlaneInfo")]
        public void UpdatePlaneInfo([FromBody] PlaneContract planeContract)
        {
            _airTrafficInfoService.UpdatePlaneInfo(planeContract);
        }

        [HttpPost]
        [Route("UpdateAirportInfo")]
        public void UpdateAirportInfo([FromBody] AirportContract airportContract)
        {
            _airTrafficInfoService.UpdateAirportInfo(airportContract);
        }
    }
}
