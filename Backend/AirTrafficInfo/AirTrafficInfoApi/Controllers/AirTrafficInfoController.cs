using AirTrafficInfoApi.Services;
using AirTrafficInfoContracts;
using Microsoft.AspNetCore.Mvc;

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
        public string Get()
        {
            return _airTrafficInfoService.GetAirTrafficInfo();
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
